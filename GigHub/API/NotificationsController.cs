using AutoMapper;
using GigHub.DTOs;
using GigHub.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

namespace GigHub.API
{
    [Authorize]
    public class NotificationsController : ApiController
    {
        private ApplicationDbContext _context;

        public NotificationsController()
        {
            _context = new ApplicationDbContext();
        }

        public IEnumerable<NotificationDto> GetNewNotifications()
        {
            var userId = User.Identity.GetUserId();

            var notifications = _context.UserNotifications
                .Where(u => u.UserId == userId && u.IsRead == false)
                .Select(un => un.Notification)
                .Include(n => n.Gig.Artist)
                .ToList();

            return notifications.Select(Mapper.Map<Notification, NotificationDto>);
        }

        [HttpPost]
        public IHttpActionResult MarkAsRead([FromBody]List<int> NotificationIds)
        {
            if (NotificationIds == null || NotificationIds.Count == 0)
                return Ok();

            var userId = User.Identity.GetUserId();
            var notifications = _context.UserNotifications
                .Where(u => u.UserId == userId 
                    && u.IsRead == false 
                    && NotificationIds.Contains(u.NotificationId))
                .ToList();

            notifications.ForEach(n => n.Read());

            _context.SaveChangesAsync();

            return Ok();
        }
    }
}
