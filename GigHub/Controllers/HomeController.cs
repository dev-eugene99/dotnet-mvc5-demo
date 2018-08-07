using GigHub.Interfaces;
using GigHub.Services;
using GigHub.ViewModels;
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

        public ActionResult Index()
        {
            var upcomingGigs = _gigService.GetUpcomingGigs();

            var gigsViewModel = new GigsViewModel
            {
                Heading = "Upcoming Gigs",
                Gigs = upcomingGigs,
                ShowActions = User.Identity.IsAuthenticated
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