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
        private readonly IGigService _gigService;

        public HomeController(IGigService gigService)
        {
            _gigService = gigService;
        }

        public ActionResult Index(string query = null)
        {
            var upcomingGigs = _gigService.GetUpcomingGigs();
            var userId = string.Empty;
            if (Request.IsAuthenticated)
            {
                userId = User.Identity.GetUserId();
            }

            if (!string.IsNullOrEmpty(query))
            {
                upcomingGigs = upcomingGigs
                    .Where(g =>
                            g.Artist.Name.Contains(query) ||
                            g.Genre.Name.Contains(query) ||
                            g.Venue.Contains(query));
            }

            var gigModels = new List<GigDetailViewModel>();
            foreach (var g in upcomingGigs)
            {
                gigModels.Add(new GigDetailViewModel(g, userId));
            };

            var gigsViewModel = new GigsViewModel
            {
                Heading = "Upcoming Gigs",
                Gigs = gigModels,
                ShowActions = User.Identity.IsAuthenticated,
                SearchTerm = query
            };

            return View("Gigs", gigsViewModel);
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