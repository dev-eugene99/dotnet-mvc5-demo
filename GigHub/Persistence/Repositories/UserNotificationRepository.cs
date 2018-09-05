using GigHub.Core.Models;
using GigHub.Core.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace GigHub.Persistence.Repositories
{
    public class UserNotificationRepository : IUserNotificationRepository
    {
        private readonly ApplicationDbContext _context;

        public UserNotificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Notification> GetNewNotifications(string userId)
        {
            return _context.UserNotifications
                .Where(u => u.UserId == userId && u.IsRead == false)
                .Select(un => un.Notification)
                .Include(n => n.Gig.Artist);
        }

        public void MarkNotificationsAsRead(string userId, List<int> notificationIds)
        {
            var userNotifications = _context.UserNotifications
                .Where(u => u.UserId == userId
                    && u.IsRead == false
                    && notificationIds.Contains(u.NotificationId))
                .ToList();

            userNotifications.ForEach(n => n.Read());
        }
    }
}