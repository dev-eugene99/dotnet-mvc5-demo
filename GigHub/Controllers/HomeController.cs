using GigHub.Core;
using GigHub.Core.Models;
using GigHub.Core.ViewModels;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index(string query = null)
        {
            var upcomingGigs = _unitOfWork.Gigs.GetUpcomingGigs(query);

            var gigsViewModel = new GigsViewModel
            {
                Heading = "Upcoming Gigs",
                Gigs = PrepareGigDetailViewModelList(upcomingGigs, GetLoggedInUserId()),
                ShowActions = User.Identity.IsAuthenticated,
                SearchTerm = query
            };

            return View("Gigs", gigsViewModel);
        }

        private static List<GigDetailViewModel> PrepareGigDetailViewModelList(IQueryable<Gig> upcomingGigs, string userId)
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