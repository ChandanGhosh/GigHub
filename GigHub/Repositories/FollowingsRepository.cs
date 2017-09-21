using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GigHub.Core.Models;
using GigHub.Core.Repositories;
using GigHub.Persistence;

namespace GigHub.Repositories
{
    public class FollowingsRepository : IFollowingsRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public FollowingsRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public Followings GetFollower(string userId, string gigArtistId)
        {
            return _dbContext.Followings.SingleOrDefault(f => f.FolloweeId == gigArtistId && f.FollowerId == userId);
        }
    }
}