using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Payment.Application.Commands.CreatePayment;
using Payment.Application.Queries.GetPaymentById;
using Payment.Infrastructure;
using Payment.Application.DTOs;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

// Add Infrastructure (DbContext + DI)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                       ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddPaymentInfrastructure(connectionString);

// Add MediatR (for Application layer commands/queries)
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<CreatePaymentCommand>();
});

//  Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//  Swagger UI
app.UseSwagger();
app.UseSwaggerUI();

//  Minimal API Endpoints

// Create Payment
app.MapPost("/payments", async (PaymentDto dto, IMediator mediator) =>
{
    var command = new CreatePaymentCommand(
        dto.BookingId,
        dto.Amount,
        dto.Currency
    );

    var result = await mediator.Send(command);
    return Results.Ok(result);
})
.WithName("CreatePayment");

// ✅ Get Payment by ID
app.MapGet("/payments/{id}", async (Guid id, IMediator mediator) =>
{
    var query = new GetPaymentByIdQuery(id);
    var result = await mediator.Send(query);
    return result is not null ? Results.Ok(result) : Results.NotFound();
})
.WithName("GetPaymentById");

app.Run();
