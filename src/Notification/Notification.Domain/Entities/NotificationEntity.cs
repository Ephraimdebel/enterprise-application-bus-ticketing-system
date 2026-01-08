using System;

namespace Notification.Domain.Entities
{
    public class NotificationEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; } // who receives the notification
        public string Message { get; set; } = null!;
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
