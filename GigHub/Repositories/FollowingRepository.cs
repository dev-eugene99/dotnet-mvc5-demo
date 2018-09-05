using GigHub.Interfaces;
using GigHub.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GigHub.Repositories
{
    public class FollowingRepository : IFollowingRepository
    {
        private readonly ApplicationDbContext _context;

        public FollowingRepository()
        {
            _context = new ApplicationDbContext();
        }

        public FollowingRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public Following GetFollowing(string followerId, string followeeId)
        {
            return _context.Followings
                .Where(a => a.FollowerId == followerId && a.FolloweeId == followeeId)
                .SingleOrDefault();
        }

        public async Task<Tuple<int, string>> AddFollowingAsync(Following following)
        {
            return await runDbActionInTryCatch(_context.Followings.Add, following);
        }

        public async Task<Tuple<int, string>> RemoveFollowingAsync(Following following)
        {
            return await runDbActionInTryCatch(_context.Followings.Remove, following);
        }

        private delegate Following FollowingDbAction(Following following);

        private async Task<Tuple<int, string>> runDbActionInTryCatch(FollowingDbAction dbAction, Following following)
        {
            var status = Tuple.Create(0, "SUCCESS");
            try
            {
                dbAction(following);
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