using AutoMapper;
using GigHub.Core;
using GigHub.Core.Models;
using GigHub.DTOs;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace GigHub.API
{
    [Authorize]
    public class NotificationsController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public NotificationsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<NotificationDto> GetNewNotifications()
        {
            var userId = User.Identity.GetUserId();

            var notifications = _unitOfWork.UserNotifications.GetNewNotifications(userId);

            return notifications.Select(Mapper.Map<Notification, NotificationDto>);
        }

        [HttpPost]
        public async Task<IHttpActionResult> MarkAsRead([FromBody]List<int> NotificationIds)
        {
            if (NotificationIds == null || NotificationIds.Count == 0)
                return Ok();

            _unitOfWork.UserNotifications
                .MarkNotificationsAsRead(User.Identity.GetUserId(), NotificationIds);

            var result = await _unitOfWork.CompleteAsync();
            if (result.Item1 == 0)
                return Ok();
            else
                return InternalServerError(new Exception(result.Item2));
        }
    }
}
