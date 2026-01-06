using MediatR;
using Microsoft.AspNetCore.Mvc;
using Notification.Application.Commands;

namespace Notification.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotificationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateNotificationCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(new { NotificationId = id });
        }

        [HttpGet("{userId}")]
        public IActionResult GetForUser(Guid userId)
        {
            // To implement: fetch notifications from DB
            return Ok(); 
        }
    }
}
