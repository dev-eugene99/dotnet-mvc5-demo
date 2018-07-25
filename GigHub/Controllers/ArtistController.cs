using GigHub.Interfaces;
using GigHub.Services;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    public class ArtistsController : Controller
    {
        private readonly IArtistsService _artistsService;

        public ArtistsController()
        {
            _artistsService = new ArtistService();
        }

        [Authorize]
        public async Task<ActionResult> Following()
        {
            var userId = User.Identity.GetUserId();
            var artists = await _artistsService.GetArtistsByFollowerIdAsync(userId);

            var artistsList = new List<ArtistViewModel>();

            foreach(var artist in artists)
            {
                artistsList.Add(new ArtistViewModel
                {
                    Id = artist.Id,
                    Name = artist.Name
                });
            }

            ArtistsViewModel viewModel = new ArtistsViewModel
            {
                Heading = "Artists I'm Following",
                ShowActions = true,
                Artists = artistsList
            };

            return View("Artists", viewModel);
        }
    }
}