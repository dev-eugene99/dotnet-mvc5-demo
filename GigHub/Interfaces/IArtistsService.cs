
using GigHub.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GigHub.Interfaces
{
    public interface IArtistsService
    {
        Task<IEnumerable<ApplicationUser>> GetArtistsByFollowerIdAsync(string followerId);
    }
}