using GigHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using GigHub.ViewModels;

namespace GigHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext context;
        public HomeController()
        {
            context = new ApplicationDbContext();
        }
        public ActionResult Index()
        {
            var upCommingGigs = context.Gigs
                .Include(a => a.Artist)
                .Include(g=>g.Genre)
                .Where(d => d.DateTime > DateTime.Now && !d.IsCanceled);
            var viewModel = new GigsViewModel
            {
                UpCommingGigs = upCommingGigs,
                ShowActions = User.Identity.IsAuthenticated,
                Heading="UpComming Gigs"
            };
            return View("Gigs",viewModel);
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