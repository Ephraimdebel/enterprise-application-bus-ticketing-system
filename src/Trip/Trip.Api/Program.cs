using MediatR;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Trip.Application.Commands.CreateTrip;
using Trip.Application.Interfaces;
using Trip.Infrastructure.Clients;
using Trip.Infrastructure.Persistence;
using Trip.Infrastructure.Repositories;
using Trip.Infrastructure.Serialization;
using Trip.Infrastructure.Outbox;
using Quartz;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// MediatR
builder.Services.AddMediatR(typeof(CreateTripCommand).Assembly);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Keycloak:Authority"];
        options.Audience = builder.Configuration["Keycloak:Audience"];
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
    });

builder.Services.AddAuthorization();

// Database
builder.Services.AddDbContext<TripDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("TripDb")
    )
);

// Repositories
builder.Services.AddScoped<ITripRepository, TripRepository>();

// BusProvider gateway (read-only integration)
var busProviderBaseUrl = builder.Configuration.GetValue<string>("BusProvider:BaseUrl") ?? "http://localhost:5031";
builder.Services.AddHttpClient<IBusProviderGateway, BusProviderClient>(client =>
{
    client.BaseAddress = new Uri(busProviderBaseUrl);
    client.Timeout = TimeSpan.FromSeconds(10);
});

// Quartz for Outbox
builder.Services.AddQuartz(q =>
{
    var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));
    q.AddJob<ProcessOutboxMessagesJob>(opts => opts.WithIdentity(jobKey));
    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("ProcessOutboxMessagesJob-trigger")
        .WithSimpleSchedule(x => x.WithIntervalInSeconds(5).RepeatForever()));
});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

// Ensure DateOnly/TimeOnly serialize consistently for Trip API responses
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNameCaseInsensitive = true;
    options.SerializerOptions.Converters.Add(new DateOnlyJsonConverter());
    options.SerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
});

var app = builder.Build();

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<TripDbContext>();
    await dbContext.Database.EnsureCreatedAsync();
}

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

