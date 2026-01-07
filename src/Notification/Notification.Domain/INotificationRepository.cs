// Notification.Domain/INotificationRepository.cs
using Notification.Domain.Entities;

namespace Notification.Domain
{
    public interface INotificationRepository
    {
        Task AddAsync(NotificationEntity notification);
    }
}
