using MediatR;
using Passenger.Application.Commands.DeletePassenger;
using Passenger.Application.Commands.RegisterPassenger;
using Passenger.Application.Commands.UpdatePassenger;
using Passenger.Application.Queries.GetPassengerById;
using Passenger.Domain.Entities;

namespace Passenger.Api.Endpoints;

public static class PassengerEndpoints
{
    public static IEndpointRouteBuilder MapPassengerEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/passengers")
            .WithTags("Passengers");

        // --- REGISTER PASSENGER: OPEN (no auth) ---
        group.MapPost("/", RegisterPassenger);

        // --- PROTECTED ROUTES ---
        group.MapGet("/{id:guid}", GetPassengerById)
             .RequireAuthorization();

        group.MapPut("/{id:guid}", UpdatePassenger)
             .RequireAuthorization();

        group.MapDelete("/{id:guid}", DeletePassenger)
             .RequireAuthorization();

        return app;
    }

    private static async Task<IResult> RegisterPassenger(
        RegisterPassengerRequest request,
        IMediator mediator,
        CancellationToken ct)
    {
        // Generate new PassengerId
        var passengerId = PassengerId.New();

        var dto = await mediator.Send(new RegisterPassengerCommand(
            PassengerId: passengerId,
            FirstName: request.FirstName,
            LastName: request.LastName,
            Email: request.Email,
            CountryCode: request.CountryCode,
            PhoneNumber: request.PhoneNumber
        ), ct);

        return Results.Created($"/passengers/{dto.Id}", dto);
    }

    private static async Task<IResult> GetPassengerById(
        Guid id,
        IMediator mediator,
        HttpContext http,
        CancellationToken ct)
    {
        EnforceSelfOrAdmin(http.User, PassengerId.FromGuid(id));

        var dto = await mediator.Send(new GetPassengerByIdQuery(PassengerId.FromGuid(id)), ct);
        return dto is null ? Results.NotFound() : Results.Ok(dto);
    }

    private static async Task<IResult> UpdatePassenger(
        Guid id,
        UpdatePassengerRequest request,
        IMediator mediator,
        HttpContext http,
        CancellationToken ct)
    {
        EnforceSelfOrAdmin(http.User, PassengerId.FromGuid(id));

        var dto = await mediator.Send(new UpdatePassengerCommand(
            PassengerId: PassengerId.FromGuid(id),
            FirstName: request.FirstName,
            LastName: request.LastName,
            Email: request.Email,
            CountryCode: request.CountryCode,
            PhoneNumber: request.PhoneNumber
        ), ct);

        return Results.Ok(dto);
    }

    private static async Task<IResult> DeletePassenger(
        Guid id,
        IMediator mediator,
        HttpContext http,
        CancellationToken ct)
    {
        EnforceSelfOrAdmin(http.User, PassengerId.FromGuid(id));

        await mediator.Send(new DeletePassengerCommand(PassengerId.FromGuid(id)), ct);
        return Results.NoContent();
    }

    private static void EnforceSelfOrAdmin(System.Security.Claims.ClaimsPrincipal user, PassengerId routeId)
    {
        // Use your Keycloak claims logic
        var roleClaim = user.Claims.FirstOrDefault(c => c.Type == "role")?.Value;

        if (roleClaim == "admin") return;

        var selfIdClaim = user.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
        if (selfIdClaim == null || selfIdClaim != routeId.Value.ToString())
            throw new UnauthorizedAccessException("You are not allowed to access this passenger.");
    }
}

// Request DTOs
public sealed record RegisterPassengerRequest(
    string FirstName,
    string LastName,
    string Email,
    string CountryCode,
    string PhoneNumber
);

public sealed record UpdatePassengerRequest(
    string FirstName,
    string LastName,
    string Email,
    string CountryCode,
    string PhoneNumber
);
