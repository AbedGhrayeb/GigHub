using AutoMapper;
using GigHub.Dtos;
using GigHub.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class NotificationsController : ApiController
    {
        private ApplicationDbContext context;
        public NotificationsController()
        {
            context = new ApplicationDbContext();
        }

        public IEnumerable<NotificationDto> GetNewNotifications()
        {
            var userId = User.Identity.GetUserId();
            var notifications = context.UserNotifications
                .Where(u => u.UserId == userId && !u.IsRead)
                .Select(n => n.Notification)
                .Include(g => g.Gig.Artist)
                .ToList();
            return notifications.Select(Mapper.Map<Notification, NotificationDto>);
        }
    }
}
