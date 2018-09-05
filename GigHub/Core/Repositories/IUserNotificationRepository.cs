using GigHub.Core.Models;
using System.Collections.Generic;

namespace GigHub.Core.Repositories
{
    public interface IUserNotificationRepository
    {
        IEnumerable<Notification> GetNewNotifications(string userId);
        void MarkNotificationsAsRead(string userId, List<int> notificationIds);
    }
}
