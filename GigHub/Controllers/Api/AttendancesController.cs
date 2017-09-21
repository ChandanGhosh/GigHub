using System.Linq;
using System.Web.Http;
using GigHub.Core.Dtos;
using GigHub.Core.Models;
using GigHub.Persistence;
using Microsoft.AspNet.Identity;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class AttendancesController : ApiController
    {
        private ApplicationDbContext _db;


        public AttendancesController()
        {
            _db = new ApplicationDbContext();
        }

        [HttpPost]
        public IHttpActionResult AddAttendance(GigDto gigDto)
        {
            var userId = User.Identity.GetUserId();

            if (_db.Attendances.Any(a => a.AttendeeId == userId && a.GigId == gigDto.Id))
            {
                BadRequest("attendance exists!");
            }

            var attendance = new Attendance()
            {
                GigId = gigDto.Id,
                AttendeeId = userId
            };

            _db.Attendances.Add(attendance);
            _db.SaveChanges();

            return Ok();
        }
        [HttpDelete]
        public IHttpActionResult DeleteAttendance(GigDto gigDto)
        {
            var userId = User.Identity.GetUserId();

            var existingAttendance = _db.Attendances.SingleOrDefault(a => a.AttendeeId == userId && a.GigId == gigDto.Id);

            if (existingAttendance == null) return NotFound();

            _db.Attendances.Remove(existingAttendance);
            _db.SaveChanges();
            return Ok(gigDto.Id);
        }
    }
}
