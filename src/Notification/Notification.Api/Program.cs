using MediatR;
using Microsoft.EntityFrameworkCore;
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

builder.Services.AddDbContext<NotificationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("NotificationDb")));


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

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
