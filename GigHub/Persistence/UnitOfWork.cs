using GigHub.Core;
using GigHub.Core.Repositories;
using GigHub.Persistence.Repositories;
using System;
using System.Threading.Tasks;

namespace GigHub.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IGigRepository Gigs { get; private set; }
        public IGenreRepository Genres { get; private set; }
        public IFollowingRepository Followings { get; private set; }
        public IArtistRepository Artists { get; private set; }
        public IAttendanceRepository Attendances { get; private set; }
        public IUserNotificationRepository UserNotifications { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Gigs = new GigRepository(context);
            Genres = new GenreRepository(context);
            Followings = new FollowingRepository(context);
            Artists = new ArtistRepository(context);
            Attendances = new AttendanceRepository(context);
            UserNotifications = new UserNotificationRepository(context);
        }

        public Tuple<int, string> Complete()
        {
            var status = Tuple.Create(0, "SUCCESS");
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                status = Tuple.Create(-1, $"FAILURE: {ex.Message}");
            }
            return status;
        }

        public async Task<Tuple<int, string>> CompleteAsync()
        {
            var status = Tuple.Create(0, "SUCCESS");
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                status = Tuple.Create(-1, $"FAILURE: {ex.Message}");
            }
            return status;
        }
    }
}