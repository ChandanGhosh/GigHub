using GigHub.Core.Models;

namespace GigHub.Core.Repositories
{
    public interface IFollowingsRepository
    {
        Followings GetFollower(string userId, string gigArtistId);
    }
}