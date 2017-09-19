using GigHub.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class GigsController : ApiController
    {
        private string _userId;
        private ApplicationDbContext _db;

        public GigsController()
        {
            _db = new ApplicationDbContext();
            _userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
        }

        [HttpDelete]
        public IHttpActionResult Cancel(int id)
        {
            var gig = _db.Gigs.Include(g => g.Attendances.Select(a => a.Attendee)).Single(g => g.Id == id && g.ArtistId == _userId);

            if (gig.IsCanceled) return NotFound();

            gig.Cancel();


            _db.SaveChanges();
            return Ok();
        }
    }
}
