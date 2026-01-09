using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using Payment.Application.DTOs;
using MediatR;
using Payment.Application.Commands.CreatePayment;

namespace Payment.Infrastructure.Messaging;

public class BookingConfirmedConsumer : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IMediator _mediator;

    public BookingConfirmedConsumer(IConnection connection, IMediator mediator)
    {
        _connection = connection;
        _mediator = mediator;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var channel = _connection.CreateModel();
        channel.ExchangeDeclare("booking.exchange", ExchangeType.Direct);
        channel.QueueDeclare("booking.confirmed.queue", durable: true, exclusive: false, autoDelete: false);
        channel.QueueBind("booking.confirmed.queue", "booking.exchange", "booking.confirmed");

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var messageJson = Encoding.UTF8.GetString(body);

            // Deserialize to local DTO
            var bookingEvent = JsonSerializer.Deserialize<BookingConfirmedDto>(messageJson);
            if (bookingEvent is null) return;

            // Send CreatePaymentCommand to Application layer
            var command = new CreatePaymentCommand(
                bookingEvent.BookingId,
                bookingEvent.TotalAmount,
                "USD" // or map currency if sent in DTO
            );

            await _mediator.Send(command, stoppingToken);
        };

        channel.BasicConsume(queue: "booking.confirmed.queue", autoAck: true, consumer: consumer);

        return Task.CompletedTask;
    }
}
