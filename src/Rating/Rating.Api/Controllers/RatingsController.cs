using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rating.Application.Commands.CreateRating;
using Rating.Application.Queries.GetRatingsByTrip;
using Rating.Application.Queries.GetRatingsByUser;
using Rating.Application.DTOs;


namespace Rating.Api.Controllers;

[ApiController]
[Route("ratings")]
public sealed class RatingsController : ControllerBase
{
    private readonly IMediator _mediator;

    public RatingsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // POST /ratings
    [HttpPost]
    public async Task<IActionResult> CreateRating([FromBody] CreateRatingRequest request)
    {
        var command = new CreateRatingCommand(
            request.TripId,
            request.UserId,
            request.TargetId,
            request.Stars,
            request.Comment
        );

        try
        {
            await _mediator.Send(command);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // GET /ratings/{tripId}
    [HttpGet("{tripId:guid}")]
    public async Task<IActionResult> GetRatingsByTrip(Guid tripId)
    {
        var result = await _mediator.Send(new GetRatingsByTripQuery(tripId));
        return Ok(result);
    }

    // GET /ratings/user/{userId}
    [HttpGet("user/{userId:guid}")]
    public async Task<IActionResult> GetRatingsByUser(Guid userId)
    {
        var result = await _mediator.Send(new GetRatingsByUserQuery(userId));
        return Ok(result);
    }
}
