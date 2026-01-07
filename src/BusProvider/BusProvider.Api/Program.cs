using BusProvider.Application;
using BusProvider.Application.BusProviders;
using BusProvider.Application.Buses;
using BusProvider.Application.Routes;
using BusProvider.Application.Schedules;
using BusProvider.Infrastructure;
using MediatR;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "BusProvider API",
        Version = "v1",
        Description = "Manage bus providers, buses, routes, and schedules."
    });

    options.OrderActionsBy(api => api.RelativePath);
});
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var busProviders = app.MapGroup("/bus-providers").WithTags("Bus Providers");

busProviders.MapGet("", async (IMediator mediator, CancellationToken ct) =>
{
    var providers = await mediator.Send(new ListBusProvidersQuery(), ct);
    return Results.Ok(providers);
});

busProviders.MapGet("/{id:guid}", async (Guid id, IMediator mediator, CancellationToken ct) =>
{
    var provider = await mediator.Send(new GetBusProviderQuery(id), ct);
    return provider is null ? Results.NotFound() : Results.Ok(provider);
});

busProviders.MapPost("", async (RegisterBusProviderRequest request, IMediator mediator, CancellationToken ct) =>
{
    var id = await mediator.Send(new RegisterBusProviderCommand(request.Name, request.Email, request.PhoneNumber, request.Address), ct);
    return Results.Created($"/bus-providers/{id}", new { id });
});

busProviders.MapPut("/{id:guid}", async (Guid id, UpdateBusProviderRequest request, IMediator mediator, CancellationToken ct) =>
{
    var updated = await mediator.Send(new UpdateBusProviderCommand(id, request.Name, request.Email, request.PhoneNumber, request.Address), ct);
    return updated ? Results.NoContent() : Results.NotFound();
});

busProviders.MapDelete("/{id:guid}", async (Guid id, IMediator mediator, CancellationToken ct) =>
{
    var deleted = await mediator.Send(new DeleteBusProviderCommand(id), ct);
    return deleted ? Results.NoContent() : Results.NotFound();
});

var buses = app.MapGroup("/buses").WithTags("Buses");

buses.MapGet("", async (Guid? providerId, IMediator mediator, CancellationToken ct) =>
{
    var result = await mediator.Send(new ListBusesQuery(providerId), ct);
    return Results.Ok(result);
});

buses.MapPost("", async (CreateBusRequest request, IMediator mediator, CancellationToken ct) =>
{
    var id = await mediator.Send(new CreateBusCommand(request.ProviderId, request.BusNumber, request.BusType, request.SeatCapacity), ct);
    return Results.Created($"/buses/{id}", new { id });
});

buses.MapGet("/{id:guid}", async (Guid id, IMediator mediator, CancellationToken ct) =>
{
    var bus = await mediator.Send(new GetBusQuery(id), ct);
    return bus is null ? Results.NotFound() : Results.Ok(bus);
});

buses.MapPut("/{id:guid}", async (Guid id, UpdateBusRequest request, IMediator mediator, CancellationToken ct) =>
{
    var updated = await mediator.Send(new UpdateBusCommand(id, request.BusNumber, request.BusType, request.SeatCapacity), ct);
    return updated ? Results.NoContent() : Results.NotFound();
});

buses.MapDelete("/{id:guid}", async (Guid id, IMediator mediator, CancellationToken ct) =>
{
    var deleted = await mediator.Send(new DeleteBusCommand(id), ct);
    return deleted ? Results.NoContent() : Results.NotFound();
});

var routes = app.MapGroup("/routes").WithTags("Routes");

routes.MapGet("", async (Guid? busId, IMediator mediator, CancellationToken ct) =>
{
    var routes = await mediator.Send(new ListRoutesQuery(busId), ct);
    return Results.Ok(routes);
});

routes.MapPost("", async (CreateRouteRequest request, IMediator mediator, CancellationToken ct) =>
{
    var id = await mediator.Send(new CreateRouteCommand(request.BusId, request.Start, request.End, request.DistanceKm), ct);
    return Results.Created($"/routes/{id}", new { id });
});

routes.MapGet("/{id:guid}", async (Guid id, IMediator mediator, CancellationToken ct) =>
{
    var route = await mediator.Send(new GetRouteQuery(id), ct);
    return route is null ? Results.NotFound() : Results.Ok(route);
});

routes.MapPut("/{id:guid}", async (Guid id, UpdateRouteRequest request, IMediator mediator, CancellationToken ct) =>
{
    var updated = await mediator.Send(new UpdateRouteCommand(id, request.Start, request.End, request.DistanceKm), ct);
    return updated ? Results.NoContent() : Results.NotFound();
});

routes.MapDelete("/{id:guid}", async (Guid id, IMediator mediator, CancellationToken ct) =>
{
    var deleted = await mediator.Send(new DeleteRouteCommand(id), ct);
    return deleted ? Results.NoContent() : Results.NotFound();
});

var schedules = app.MapGroup("/schedules").WithTags("Schedules");

schedules.MapGet("", async (Guid? busId, Guid? routeId, IMediator mediator, CancellationToken ct) =>
{
    var schedules = await mediator.Send(new ListSchedulesQuery(busId, routeId), ct);
    return Results.Ok(schedules);
});

schedules.MapPost("", async (CreateScheduleRequest request, IMediator mediator, CancellationToken ct) =>
{
    var id = await mediator.Send(new CreateScheduleCommand(request.BusId, request.RouteId, request.TripDate, request.Departure, request.Arrival, request.SeatsAvailable), ct);
    return Results.Created($"/schedules/{id}", new { id });
});

schedules.MapGet("/{id:guid}", async (Guid id, IMediator mediator, CancellationToken ct) =>
{
    var schedule = await mediator.Send(new GetScheduleQuery(id), ct);
    return schedule is null ? Results.NotFound() : Results.Ok(schedule);
});

schedules.MapPut("/{id:guid}", async (Guid id, UpdateScheduleRequest request, IMediator mediator, CancellationToken ct) =>
{
    var updated = await mediator.Send(new UpdateScheduleCommand(id, request.TripDate, request.Departure, request.Arrival, request.SeatsAvailable), ct);
    return updated ? Results.NoContent() : Results.NotFound();
});

schedules.MapDelete("/{id:guid}", async (Guid id, IMediator mediator, CancellationToken ct) =>
{
    var deleted = await mediator.Send(new DeleteScheduleCommand(id), ct);
    return deleted ? Results.NoContent() : Results.NotFound();
});

app.Run();

public record RegisterBusProviderRequest(string Name, string Email, string PhoneNumber, string Address);
public record UpdateBusProviderRequest(string Name, string Email, string PhoneNumber, string Address);
public record CreateBusRequest(Guid ProviderId, string BusNumber, string BusType, int SeatCapacity);
public record UpdateBusRequest(string BusNumber, string BusType, int SeatCapacity);
public record CreateRouteRequest(Guid BusId, string Start, string End, double DistanceKm);
public record UpdateRouteRequest(string Start, string End, double DistanceKm);
public record CreateScheduleRequest(Guid BusId, Guid RouteId, DateOnly TripDate, TimeOnly Departure, TimeOnly Arrival, int SeatsAvailable);
public record UpdateScheduleRequest(DateOnly TripDate, TimeOnly Departure, TimeOnly Arrival, int SeatsAvailable);
