using GigHub.Models;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    public class GigsController : Controller
    {
        private ApplicationDbContext context;

        public GigsController()
        {
            context = new ApplicationDbContext();
        }

        [Authorize]
        public ActionResult Mine()
        {
            var userId = User.Identity.GetUserId();
            var gigs = context.Gigs
                .Where(g => g.ArtistId == userId &&
                g.DateTime > DateTime.Now &&
                !g.IsCanceled)
                .Include(g=>g.Genre)
                .ToList();
            return View(gigs);
        }
        [Authorize]
        public ActionResult Attending()
        {
            var userId = User.Identity.GetUserId();
            var gigs = context.Attendances
                .Where(a => a.AttendeeId == userId)
                .Select(a => a.Gig)
                .Include(a=>a.Artist)
                .Include(g=>g.Genre)
                .ToList();

            var viewModel = new GigsViewModel
            {
                UpCommingGigs = gigs,
                ShowActions = User.Identity.IsAuthenticated,
                Heading="Gigs I'm Attending"
            };
            return View("Gigs",viewModel);
        } [Authorize]
        public ActionResult Following()
        {
            var userId = User.Identity.GetUserId();
            var artist = context.Followings
                .Where(a => a.FollowerId == userId)
                .Select(a => a.Followee.Name)
                
                .ToList();

            var viewModel = new ArtistViewModel
            {
                FollowingArtists = artist,
                ShowActions = User.Identity.IsAuthenticated,
                Heading="Artist I'm Following"
            };
            return View(viewModel);
        }
        [Authorize]
        public ActionResult Create()
        {
            GigFormViewModel viewModel = new GigFormViewModel
            {
                Heading="Add a Gig",
                Genres = context.Genres.ToList()
            };
            return View("GigForm",viewModel);
        }
           public ActionResult Edit(int id)
        {
            var userId = User.Identity.GetUserId();
            var gig = context.Gigs.Single(g => g.Id == id && g.ArtistId == userId);

            var viewModel = new GigFormViewModel
            {
               Heading="Edit a Gig",
               Id=gig.Id,
                Genres = context.Genres.ToList(),
                Date = gig.DateTime.ToString("d MMM yyyy"),
                Time=gig.DateTime.ToString("HH:mm"),
                Venue = gig.Venue,
                Genre = gig.GenreId

            };
            return View("GigForm",viewModel);
        }

        [Authorize]
        [HttpPost][ValidateAntiForgeryToken]
        public ActionResult Create(GigFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = context.Genres.ToList();
                return View("GigForm", viewModel);
            }
  
            Gig gig = new Gig
            {
                ArtistId = User.Identity.GetUserId(),
                DateTime =viewModel.GetDateTime(),
                Venue = viewModel.Venue,
                GenreId = viewModel.Genre
            };
            context.Gigs.Add(gig);
            context.SaveChanges();
            return View(nameof(Mine));
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(GigFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = context.Genres.ToList();
                return View("GigForm", viewModel);
            }

            var userId = User.Identity.GetUserId();
            var gig = context.Gigs
                .Include(a=>a.Attendances.Select(g=>g.Attendee))
                .Single(g => g.Id == viewModel.Id && g.ArtistId == userId);
            gig.Modify(viewModel.GetDateTime(), viewModel.Venue, viewModel.Genre);
           

            context.SaveChanges();
            return View("Mine","Gigs");
        }

    }
}