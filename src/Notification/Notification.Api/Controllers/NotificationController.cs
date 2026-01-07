using MediatR;
using Microsoft.AspNetCore.Mvc;
using Notification.Application.Commands;
using Notification.Domain;

namespace Notification.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly INotificationRepository _repository;

    public NotificationsController(IMediator mediator, INotificationRepository repository)
    {
        _mediator = mediator;
        _repository = repository;
    }

    // Keep your existing POST endpoint
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateNotificationCommand command)
    {
        var id = await _mediator.Send(command);
        return Ok(new { NotificationId = id });
    }

    // New: GET all notifications
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var notifications = await _repository.GetAllAsync();
        return Ok(notifications);
    }

    // Optional: GET notifications for a specific user
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetForUser(Guid userId)
    {
        var notifications = (await _repository.GetAllAsync())
                            .Where(n => n.UserId == userId) // Assuming NotificationEntity has UserId
                            .ToList();
        return Ok(notifications);
    }
}
