using GigHub.Interfaces;
using GigHub.Services;
using GigHub.ViewModels;
using System.Linq;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGigService _gigService;

        public HomeController()
        {
            _gigService = new GigService();
        }

        public ActionResult Index(string query = null)
        {
            var upcomingGigs = _gigService.GetUpcomingGigs();

            if (!string.IsNullOrEmpty(query))
            {
                upcomingGigs = upcomingGigs
                    .Where(g =>
                            g.Artist.Name.Contains(query) ||
                            g.Genre.Name.Contains(query) ||
                            g.Venue.Contains(query));
            }
            var gigsViewModel = new GigsViewModel
            {
                Heading = "Upcoming Gigs",
                Gigs = upcomingGigs,
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