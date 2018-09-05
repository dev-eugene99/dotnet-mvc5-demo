using GigHub.Core;
using GigHub.Core.Models;
using GigHub.Core.ViewModels;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    public class GigsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public GigsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new GigFormViewModel()
            {
                Heading = "Add a Gig",
                Genres = _unitOfWork.Genres.GetGenres()
            };

            return View("GigForm", viewModel);
        }

        [HttpPost]
        public ActionResult Search(GigsViewModel viewModel)
        {
            return RedirectToAction("Index", "Home", new { query = viewModel.SearchTerm });
        }

        public async Task<ActionResult> Details(int id)
        {
            var gig = await _unitOfWork.Gigs.GetGigDetailByIdAsync(id);
            if (gig == null)
                return HttpNotFound();

            string userId = GetCurrentUserId();
            return View(new GigDetailViewModel(gig, userId));
        }

        private string GetCurrentUserId()
        {
            var userId = string.Empty;
            if (Request.IsAuthenticated)
            {
                userId = User.Identity.GetUserId();
            }

            return userId;
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(GigFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = _unitOfWork.Genres.GetGenres();
                return View("GigForm", viewModel);
            }

            var gig = new Gig(GetCurrentUserId(), viewModel.GetDateTime(), viewModel.Genre, viewModel.Venue);

            _unitOfWork.Gigs.AddGig(gig);

            var msg = await _unitOfWork.CompleteAsync();

            if (msg.Item1 == 0)
                return RedirectToAction("Mine", "Gigs");
            else
                return View("GigForm", viewModel);
        }

        [Authorize]
        public async Task<ActionResult> Edit(int id)
        {
            var gig = await _unitOfWork.Gigs.GetGigAsync(id);

            if (gig == null)
                return HttpNotFound();

            if (gig.ArtistId != User.Identity.GetUserId())
                return new HttpUnauthorizedResult();

            return View("GigForm", new GigFormViewModel(gig, "Edit a Gig", _unitOfWork.Genres.GetGenres()));
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(GigFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = _unitOfWork.Genres.GetGenres();
                return View("Mine", viewModel);
            }

            var gig = await _unitOfWork.Gigs.GetGigAsync(viewModel.Id);

            if (gig == null)
                return HttpNotFound();

            if (gig.ArtistId != User.Identity.GetUserId())
                return new HttpUnauthorizedResult();

            _unitOfWork.Gigs
                .UpdateGig(gig, viewModel.GetDateTime(), viewModel.Genre, viewModel.Venue);

            var msg = await _unitOfWork.CompleteAsync();

            return RedirectToAction("Mine", "Gigs");
        }

        [Authorize]
        public async Task<ActionResult> Mine()
        {
            return View(await _unitOfWork.Gigs.GetUpcomingGigsByArtistIdAsync(User.Identity.GetUserId()));
        }

        [Authorize]
        public async Task<ActionResult> Attending()
        {
            var userId = User.Identity.GetUserId();
            var gigs = _unitOfWork.Gigs.GetGigsByAttendeeIdAsync(userId);
            return View("Gigs", await PrepareGigsViewModel(userId, gigs));
        }

        private static async Task<GigsViewModel> PrepareGigsViewModel(string userId, Task<System.Collections.Generic.IList<Gig>> gigs)
        {
            return new GigsViewModel
            {
                Heading = "Gigs I'm Attending",
                Gigs = (await gigs).Select(g => new GigDetailViewModel(g, userId)).ToList(),
                ShowActions = false
            };
        }
    }
}