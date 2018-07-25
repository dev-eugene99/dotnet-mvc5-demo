using GigHub.Interfaces;
using GigHub.Models;
using GigHub.Services;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    public class GigsController : Controller
    {
        private readonly IGigService _gigService;

        public GigsController()
        {
            _gigService = new GigServices();
        }

        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new GigFormViewModel()
            {
                Genres = _gigService.GetGenres()
            };

            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GigFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = _gigService.GetGenres();
                return View("Create", viewModel);
            }

            var gig = new Gig
            {
                ArtistId = User.Identity.GetUserId(),
                DateTime = viewModel.GetDateTime(),
                GenreId = viewModel.Genre,
                Venue = viewModel.Venue
            };
            var msg = _gigService.AddGig(gig);

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<ActionResult> Mine()
        {
            var userId = User.Identity.GetUserId();
            var gigs = await _gigService.GetUpcomingGigsByArtistIdAsync(userId);

            var view = new GigsViewModel
            {
                Heading = "My Upcoming Gigs",
                Gigs = gigs,
                ShowActions = false
            };

            return View("Gigs", view);
        }

        [Authorize]
        public async Task<ActionResult> Attending()
        {
            var userId = User.Identity.GetUserId();
            var gigs = await _gigService.GetGigsByAttendeeIdAsync(userId);

            var view = new GigsViewModel
            {
                Heading = "Gigs I'm Attending",
                Gigs = gigs,
                ShowActions = false
            };

            return View("Gigs", view);
        }
    }
}