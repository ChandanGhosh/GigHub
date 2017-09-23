using GigHub.Core.Repositories;

namespace GigHub.Core
{
    public interface IUnitOfWork
    {
        IGigsRepositories Gigs { get; set; }
        IFollowingsRepository Followings { get; set; }
        IGenreRepository Genres { get; set; }
        IAttendanceRepository Attendances { get; set; }
        void Complete();
    }
}