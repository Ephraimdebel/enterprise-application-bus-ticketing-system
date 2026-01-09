using MediatR;
using Microsoft.AspNetCore.Mvc;
using Trip.Application.Commands.CreateTrip;
using Trip.Application.Commands.UpdateTrip;
using Trip.Application.Commands.CancelTrip;
using Trip.Application.Commands.ReserveSeat;
using Trip.Application.Commands.ReleaseSeat;
using Trip.Application.Queries.GetTripById;
using Trip.Application.Queries.GetTripSeats;
using Trip.Application.Commands.CompleteTrip;
using Trip.Domain.ValueObjects;

namespace Trip.Api.Controllers;

[ApiController]
[Route("trips")]
public sealed class TripsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TripsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // POST /trips
    [HttpPost]
    public async Task<IActionResult> CreateTrip([FromBody] CreateTripRequest request)
    {
        var tripId = Guid.NewGuid();
        var command = new CreateTripCommand(
            tripId,
            request.BusId,
            request.RouteId,
            new TravelDateTime(request.DepartureDate, request.DepartureTime),
            new TravelDateTime(request.ArrivalDate, request.ArrivalTime),
            new TripPrice(request.Price)
        );

        await _mediator.Send(command);

        return CreatedAtAction(
            nameof(GetTripById),
            new { id = tripId },
            new { id = tripId }
        );
    }


    // GET /trips/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetTripById(Guid id)
    {
        var result = await _mediator.Send(new GetTripByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    // PUT /trips/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateTrip(Guid id, [FromBody] UpdateTripRequest request)
    {
        var command = new UpdateTripCommand(
            id,
            new TravelDateTime(request.NewDepartureDate, request.NewDepartureTime),
            new TravelDateTime(request.NewArrivalDate, request.NewArrivalTime),
            new TripPrice(request.NewPrice)
        );

        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> CancelTrip(Guid id)
    {
        try
        {
            await _mediator.Send(new CancelTripCommand(id));
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // POST /trips/{id}/complete
    [HttpPost("{id:guid}/complete")]
    public async Task<IActionResult> CompleteTrip(Guid id)
    {
        try
        {
            await _mediator.Send(new CompleteTripCommand(id));
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // GET /trips/{id}/seats
    [HttpGet("{id:guid}/seats")]
    public async Task<IActionResult> GetTripSeats(Guid id)
    {
        var seats = await _mediator.Send(new GetTripSeatsQuery(id));
        return Ok(seats);
    }
}
