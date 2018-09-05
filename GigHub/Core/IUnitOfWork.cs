using GigHub.Core.Repositories;
using System;
using System.Threading.Tasks;

namespace GigHub.Core
{
    public interface IUnitOfWork
    {
        IGigRepository Gigs { get; }
        IGenreRepository Genres { get; }
        IFollowingRepository Followings { get; }
        IArtistRepository Artists { get; }
        IAttendanceRepository Attendances { get; }
        IUserNotificationRepository UserNotifications { get; }

        Tuple<int, string> Complete();
        Task<Tuple<int, string>> CompleteAsync();        
    }
}
