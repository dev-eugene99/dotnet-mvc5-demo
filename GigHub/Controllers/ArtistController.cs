using GigHub.Core;
using GigHub.Core.Models;
using GigHub.Core.Repositories;
using GigHub.Core.ViewModels;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    public class ArtistsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ArtistsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Authorize]
        public async Task<ActionResult> Following()
        {
            var userId = User.Identity.GetUserId();
            var artists = _unitOfWork.Artists.GetArtistsByFollowerIdAsync(userId);

            ArtistsViewModel viewModel = new ArtistsViewModel
            {
                Heading = "Artists I'm Following",
                ShowActions = false,
                Artists = PrepareArtistViewModelList(await artists)
            };

            return View("Artists", viewModel);
        }

        private static List<ArtistViewModel> PrepareArtistViewModelList(IEnumerable<ApplicationUser> artists)
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