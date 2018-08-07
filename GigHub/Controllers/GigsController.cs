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
            _gigService = new GigService();
        }

        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new GigFormViewModel()
            {
                Heading = "Add a Gig",
                Genres = _gigService.GetGenres()
            };

            return View("GigForm", viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(GigFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = _gigService.GetGenres();
                return View("GigForm", viewModel);
            }

            var gig = new Gig
            {
                ArtistId = User.Identity.GetUserId(),
                DateTime = viewModel.GetDateTime(),
                GenreId = viewModel.Genre,
                Venue = viewModel.Venue
            };
            var msg = await _gigService.AddGigAsync(gig);

            return RedirectToAction("Mine", "Gigs");
        }

        [Authorize]
        public async Task<ActionResult> Edit(int id)
        {
            var userId = User.Identity.GetUserId();
            var gig = await _gigService.GetGigByIdAsync(id);
            if (gig != null)
            {
                if (gig.ArtistId != userId)
                {
                    return Content("Unauthorized Edit Request");
                }

                var viewModel = new GigFormViewModel()
                {
                    Heading = "Edit a Gig",
                    Id = gig.Id,
                    Genres = _gigService.GetGenres(),
                    Date = gig.DateTime.ToString("d MMM yyyy"),
                    Time = gig.DateTime.ToString("HH:mm"),
                    Genre = gig.GenreId,
                    Venue = gig.Venue
                };

                return View("GigForm", viewModel);
            }
            else
            {
                return Content("Gig not found");
            }
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(GigFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = _gigService.GetGenres();
                return View("Mine", viewModel);
            }

            var userId = User.Identity.GetUserId();

            var gig = await _gigService.GetGigByIdAsync(viewModel.Id);
            if (gig != null && gig.ArtistId == userId)
            {
                gig.DateTime = viewModel.GetDateTime();
                gig.GenreId = viewModel.Genre;
                gig.Venue = viewModel.Venue;

                var msg = _gigService.UpdateGigAsync(gig);
            }

            return RedirectToAction("Mine", "Gigs");
        }

        [Authorize]
        public async Task<ActionResult> Mine()
        {
            var userId = User.Identity.GetUserId();
            var gigs = await _gigService.GetUpcomingGigsByArtistIdAsync(userId);

            return View(gigs);
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