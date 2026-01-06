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

        // --- REGISTER PASSENGER: OPEN ---
        group.MapPost("/", RegisterPassenger);

        // --- LOGIN: OPEN ---
        group.MapPost("/login", Login);

        // --- PROTECTED ROUTES ---
        group.MapGet("/{id:guid}", GetPassengerById)
             .RequireAuthorization();

        group.MapPut("/{id:guid}", UpdatePassenger)
             .RequireAuthorization();

        group.MapDelete("/{id:guid}", DeletePassenger)
             .RequireAuthorization();

        return app;
    }

    // --- REGISTER PASSENGER ---
    private static async Task<IResult> RegisterPassenger(
        RegisterPassengerRequest request,
        IMediator mediator,
        CancellationToken ct)
    {
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

    // --- LOGIN ---
    private static async Task<IResult> Login(
        LoginRequest request,
        IMediator mediator,
        CancellationToken ct)
    {
        // Here you would validate email + password against your DB
        // For demo, we just generate a JWT token if the email exists

        var passenger = await mediator.Send(new GetPassengerByEmailQuery(request.Email), ct);
        if (passenger == null)
            return Results.Unauthorized();

        // TODO: Validate password properly here
        // For now, just issue a JWT token
        var token = JwtTokenHelper.GenerateToken(passenger.Id, passenger.Email, "user");

        return Results.Ok(new { AccessToken = token });
    }

    // --- GET ---
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

    // --- UPDATE ---
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

    // --- DELETE ---
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
        var roleClaim = user.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
        if (roleClaim == "admin") return;

        var selfIdClaim = user.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
        if (selfIdClaim == null || selfIdClaim != routeId.Value.ToString())
            throw new UnauthorizedAccessException("You are not allowed to access this passenger.");
    }
}

// --- DTOs ---
public sealed record RegisterPassengerRequest(string FirstName, string LastName, string Email, string CountryCode, string PhoneNumber);
public sealed record UpdatePassengerRequest(string FirstName, string LastName, string Email, string CountryCode, string PhoneNumber);
public sealed record LoginRequest(string Email, string Password);
