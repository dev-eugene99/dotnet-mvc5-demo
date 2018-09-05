using GigHub.Interfaces;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGigRepository _gigRepository;

        public HomeController(IGigRepository gigRepository)
        {
            _gigRepository = gigRepository;
        }

        public ActionResult Index(string query = null)
        {
            var upcomingGigs = _gigRepository.GetUpcomingGigs();
            string userId = GetLoggedInUserId();

            if (!string.IsNullOrEmpty(query))
                upcomingGigs = FilterGigs(query, upcomingGigs);

            var gigsViewModel = new GigsViewModel
            {
                Heading = "Upcoming Gigs",
                Gigs = PrepareGigDetailViewModelList(upcomingGigs, userId),
                ShowActions = User.Identity.IsAuthenticated,
                SearchTerm = query
            };

            return View("Gigs", gigsViewModel);
        }

        private static List<GigDetailViewModel> PrepareGigDetailViewModelList(IQueryable<Models.Gig> upcomingGigs, string userId)
        {
            var gigModels = new List<GigDetailViewModel>();
            foreach (var g in upcomingGigs)
            {
                gigModels.Add(new GigDetailViewModel(g, userId));
            };
            return gigModels;
        }

        private string GetLoggedInUserId()
        {
            var userId = string.Empty;
            if (Request.IsAuthenticated)
            {
                userId = User.Identity.GetUserId();
            }

            return userId;
        }

        private static IQueryable<Models.Gig> FilterGigs(string query, IQueryable<Models.Gig> upcomingGigs)
        {
            upcomingGigs = upcomingGigs
                .Where(g =>
                        g.Artist.Name.Contains(query) ||
                        g.Genre.Name.Contains(query) ||
                        g.Venue.Contains(query));
            return upcomingGigs;
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}