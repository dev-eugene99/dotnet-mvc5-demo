using GigHub.Interfaces;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    public class ArtistsController : Controller
    {
        private readonly IArtistRepository _artistRepository;

        public ArtistsController(IArtistRepository artistRepository)
        {
            _artistRepository = artistRepository;
        }

        [Authorize]
        public async Task<ActionResult> Following()
        {
            var userId = User.Identity.GetUserId();
            var artists = _artistRepository.GetArtistsByFollowerIdAsync(userId);

            ArtistsViewModel viewModel = new ArtistsViewModel
            {
                Heading = "Artists I'm Following",
                ShowActions = false,
                Artists = PrepareArtistViewModelList(await artists)
            };

            return View("Artists", viewModel);
        }

        private static List<ArtistViewModel> PrepareArtistViewModelList(IEnumerable<Models.ApplicationUser> artists)
        {
            var artistsList = new List<ArtistViewModel>();

            foreach (var artist in artists)
            {
                artistsList.Add(new ArtistViewModel
                {
                    Id = artist.Id,
                    Name = artist.Name
                });
            }

            return artistsList;
        }
    }
}