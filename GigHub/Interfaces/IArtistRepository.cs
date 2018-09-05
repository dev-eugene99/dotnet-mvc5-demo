
using GigHub.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GigHub.Interfaces
{
    public interface IArtistRepository
    {
        Task<IEnumerable<ApplicationUser>> GetArtistsByFollowerIdAsync(string followerId);
    }
}