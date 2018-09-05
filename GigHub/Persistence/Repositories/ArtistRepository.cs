using GigHub.Core.Models;
using GigHub.Core.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace GigHub.Persistence.Repositories
{
    public class ArtistRepository : IArtistRepository
    {
        private readonly ApplicationDbContext _context;

        public ArtistRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ApplicationUser>> GetArtistsByFollowerIdAsync(string followerId)
        {
            return await _context.Followings
                .Where(f => f.FollowerId == followerId)
                .Select(f => f.Followee)
                .ToListAsync(); ;
        }
    }
}