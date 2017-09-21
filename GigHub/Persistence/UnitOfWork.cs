using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GigHub.Core;
using GigHub.Core.Models;
using GigHub.Core.Repositories;
using GigHub.Repositories;

namespace GigHub.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;

        public IGigsRepositories Gigs { get; set; }
        public IFollowingsRepository Followings { get; set; }
        public IGenreRepository Genres { get; set; }
        public IAttendanceRepository Attendances { get; set; }


        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            Gigs = new GigsRepositories(dbContext);
            Followings=new FollowingsRepository(dbContext);
            Genres = new GenreRepository(dbContext);
            Attendances = new AttendanceRepository(dbContext);
        }

        public void Complete()
        {
            _dbContext.SaveChanges();
        }
    }
}