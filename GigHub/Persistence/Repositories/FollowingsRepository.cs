using System.Linq;
using GigHub.Core.Models;
using GigHub.Core.Repositories;

namespace GigHub.Persistence.Repositories
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