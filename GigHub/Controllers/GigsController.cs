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

        [HttpPost]
        public ActionResult Search(GigsViewModel viewModel)
        {
            return RedirectToAction("Index", "Home", new { query = viewModel.SearchTerm });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(GigFormViewModel viewModel)
        {
            var userId = User.Identity.GetUserId();
            if (!ModelState.IsValid)
            {
                viewModel.Genres = _gigService.GetGenres();
                return View("GigForm", viewModel);
            }

            var gig = new Gig(userId, viewModel.GetDateTime(), viewModel.Genre, viewModel.Venue);
            var msg = await _gigService.AddGigAsync(gig);
            if (msg.Item1 == 0)
            {
                return RedirectToAction("Mine", "Gigs");
            }
            else
            {
                return View("GigForm", viewModel);
            }
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
                var msg = await _gigService
                    .UpdateGigAsync(gig, viewModel.GetDateTime(), viewModel.Genre, viewModel.Venue);
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