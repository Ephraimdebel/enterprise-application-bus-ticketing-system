using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Notification.Application;
using Notification.Domain;
using Notification.Infrastructure;
using Notification.Infrastructure.Messaging;
using Notification.Infrastructure.Persistence;
using RabbitMQ.Client; // Make sure this points to your Application project

var builder = WebApplication.CreateBuilder(args);

// Add MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Notification.Application.Class1).Assembly)
);



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();

builder.Services.AddNotificationInfrastructure(
    builder.Configuration.GetConnectionString("NotificationDb")
);

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


builder.Services.AddSingleton<IConnection>(static sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var section = config.GetSection("RabbitMQ");

    var factory = new ConnectionFactory
{
    HostName = section["Host"],
    Port = config.GetValue<int>("RabbitMQ:Port", 5673),
    UserName = section["User"],
    Password = section["Password"]
};


    return factory.CreateConnection();
});

builder.Services.AddHostedService<PaymentCompletedConsumer>();
builder.Services.AddHostedService<BookingConfirmedConsumer>();
builder.Services.AddHostedService<TripScheduleUpdatedConsumer>();


var app = builder.Build();

// Automatically create database and run migrations
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<Notification.Infrastructure.Persistence.NotificationDbContext>();
    context.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
