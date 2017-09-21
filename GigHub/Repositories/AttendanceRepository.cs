using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GigHub.Core.Models;
using GigHub.Core.Repositories;
using GigHub.Persistence;

namespace GigHub.Repositories
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AttendanceRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public IEnumerable<Attendance> GetFutureAttendances(string userId)
        {
            return _dbContext.Attendances
                .Where(a => a.Attendee.Id == userId && a.Gig.DateTime > DateTime.Now).ToList();
        }

        public Attendance GetAttendance(int gigId, string userId)
        {
            return _dbContext.Attendances.SingleOrDefault(a => a.AttendeeId == userId && a.GigId == gigId);
        }
    }
}