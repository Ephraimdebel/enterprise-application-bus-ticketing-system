using MediatR;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Trip.Application.Commands.CreateTrip;
using Trip.Application.Interfaces;
using Trip.Infrastructure.Clients;
using Trip.Infrastructure.Persistence;
using Trip.Infrastructure.Repositories;
using Trip.Infrastructure.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// MediatR
builder.Services.AddMediatR(typeof(CreateTripCommand).Assembly);

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

// Ensure DateOnly/TimeOnly serialize consistently for Trip API responses
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNameCaseInsensitive = true;
    options.SerializerOptions.Converters.Add(new DateOnlyJsonConverter());
    options.SerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
});

var app = builder.Build();

// Ensure database exists and apply migrations on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TripDbContext>();
    await db.Database.MigrateAsync();
}

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

