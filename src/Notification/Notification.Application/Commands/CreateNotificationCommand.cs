using MediatR;
using System;

namespace Notification.Application.Commands
{
    public record CreateNotificationCommand(Guid UserId, string Message) : IRequest<Guid>;
}
