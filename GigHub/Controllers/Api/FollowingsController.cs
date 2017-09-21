using System.Linq;
using System.Web.Http;
using GigHub.Core.Models;
using GigHub.Persistence;
using Microsoft.AspNet.Identity;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class FollowingsController : ApiController
    {
        private ApplicationDbContext _db;

        public FollowingsController()
        {
            _db = new ApplicationDbContext();
        }


        [HttpPost]
        public IHttpActionResult Follow(FollowingsDto dto)
        {
            var userId = User.Identity.GetUserId();

            var exists = _db.Followings.Any(f =>
                f.FollowerId == userId && f.FolloweeId == dto.ArtistId); // followee means the artist, user trying to follow;
            if (exists)
                return BadRequest("you are already folloing the artist");

            var following = new Followings()
            {
                FolloweeId = dto.ArtistId,
                FollowerId = userId
            };

            _db.Followings.Add(following);
            _db.SaveChanges();

            return Ok();

        }
    }
}