using Microsoft.EntityFrameworkCore;
using Notification.Domain;
using Notification.Domain.Entities;
using Notification.Infrastructure.Persistence;

namespace Notification.Infrastructure;

public class NotificationRepository : INotificationRepository
{
    private readonly NotificationDbContext _context;

    public NotificationRepository(NotificationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(NotificationEntity notification)
    {
        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();
    }

    public async Task<List<NotificationEntity>> GetAllAsync()
    {
        return await _context.Notifications
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
    }
}
