using AutoMapper;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GigHub.Core.Dtos;
using GigHub.Core.Models;
using GigHub.Persistence;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class NotificationsController : ApiController
    {
        private ApplicationDbContext _db;
        private readonly string _userId;

        public NotificationsController()
        {
            _db = new ApplicationDbContext();
            _userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
        }


        public IEnumerable<NotificationDto> GetNotifications()
        {
            var notifications = _db.UserNotifications.Where(un => un.UserId == _userId && !un.IsRead).Select(un => un.Notification)
                .Include(n => n.Gig.Artist).ToList();

            return notifications.Select(n => Mapper.Instance.Map<Notification, NotificationDto>(n));
        }
        [HttpPost]
        public IHttpActionResult MarkAsRead()
        {
            var notifications = _db.UserNotifications.Where(un => un.UserId == _userId && !un.IsRead).ToList();

            notifications.ForEach(n => n.Read());

            _db.SaveChanges();

            return Ok();
        }
    }
}
