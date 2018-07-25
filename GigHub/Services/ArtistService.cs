using GigHub.Interfaces;
using GigHub.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace GigHub.Services
{
    public class ArtistService : IArtistsService
    {
        private readonly ApplicationDbContext _context;

        public ArtistService()
        {
            _context = new ApplicationDbContext();
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