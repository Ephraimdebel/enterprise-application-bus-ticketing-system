// Notification.Application/Handlers/CreateNotificationCommandHandler.cs
using MediatR;
using Notification.Application.Commands;
using Notification.Domain;
using Notification.Domain.Entities;

namespace Notification.Application.Handlers
{
    public class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand, Guid>
    {
        private readonly INotificationRepository _repository;

        public CreateNotificationCommandHandler(INotificationRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
        {
            var notification = new NotificationEntity
            {
                Id = Guid.NewGuid(),
                Message = request.Message,
                CreatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(notification);

            return notification.Id;
        }
    }
}
