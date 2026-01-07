using MediatR;
using Microsoft.EntityFrameworkCore;
using Trip.Application.Interfaces;
using Trip.Application.Commands.CreateTrip;
using Trip.Infrastructure.Persistence;
using Trip.Infrastructure.Repositories;

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

var app = builder.Build();

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

