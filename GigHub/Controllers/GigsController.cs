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
        private readonly ApplicationDbContext _db;
        private readonly string _userId;

        public GigsController()
        {
            _db = new ApplicationDbContext();
            _userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
        }



        [Authorize]
        public ActionResult Mine()
        {

            var gigs = _db.Gigs.Where(g => g.ArtistId == _userId && g.DateTime > DateTime.Now && !g.IsCanceled).Include(g => g.Genre).ToList();

            return View(gigs);
        }


        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new GigFormViewModel
            {
                Genres = _db.Genres.ToList(),
                Heading = "Add a Gig"
            };

            return View("GigForm", viewModel);
        }
        [Authorize]
        public ActionResult Edit(int id)
        {
            var gig = _db.Gigs.Single(g => g.Id == id && g.ArtistId == _userId);


            var viewModel = new GigFormViewModel
            {
                Id = gig.Id,
                Genres = _db.Genres.ToList(),
                Date = gig.DateTime.ToString("d MMM yyyy"),
                Time = gig.DateTime.ToString("HH:mm"),
                Genre = gig.GenreId,
                Venue = gig.Venue,
                Heading = "Edit a Gig"
            };

            return View("GigForm", viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GigFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = _db.Genres.ToList();
                return View("GigForm", viewModel);
            }

            var gig = new Gig()
            {
                ArtistId = _userId,
                DateTime = viewModel.GetDateTime(),
                GenreId = viewModel.Genre,
                Venue = viewModel.Venue
            };

            _db.Gigs.Add(gig);
            _db.SaveChanges();
            return RedirectToAction("Mine", "Gigs");
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(GigFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = _db.Genres.ToList();
                return View("GigForm", viewModel);
            }


            var gig = _db.Gigs.Include(g => g.Attendances.Select(a => a.Attendee))
                .Single(g => g.Id == viewModel.Id && g.ArtistId == _userId);

            gig.Modify(viewModel.GetDateTime(), viewModel.Venue, viewModel.Genre);

            _db.SaveChanges();
            return RedirectToAction("Mine", "Gigs");
        }

        public ActionResult Attending()
        {
            var gigs = _db.Attendances
                .Where(a => a.AttendeeId == _userId)
                .Select(a => a.Gig)
                .Include(a => a.Artist)
                .Include(a => a.Genre)
                .ToList();

            var attendances = _db.Attendances
                .Where(a => a.Attendee.Id == _userId && a.Gig.DateTime > DateTime.Now).ToList().ToLookup(a => a.GigId);

            var gigsViewModel = new GigsViewModel()
            {
                UpcomingGigs = gigs,
                ShowAction = User.Identity.IsAuthenticated,
                Heading = "Gigs I'm going",
                Attendances = attendances
            };

            return View("Gigs", gigsViewModel);
        }

        [HttpPost]
        public ActionResult Search(GigsViewModel gigsViewModel)
        {
            return RedirectToAction("Index", "Home", new { query = gigsViewModel.SearchTerm });
        }

        
        public ActionResult Details(int id)
        {
            var gig = _db.Gigs.Include(g=> g.Artist).SingleOrDefault(g => g.Id == id);

            if (gig == null) return HttpNotFound();

            var gigDetailsViewModel = new GigsDetailsViewModel()
            {
                Gig = gig,
            };

            if (User.Identity.IsAuthenticated)
            {
                var isFollowing = _db.Followings.Any(f => f.FolloweeId == gig.ArtistId && f.FollowerId == _userId);
                var isAttending = _db.Attendances.Any(a => a.AttendeeId == _userId && a.GigId == gig.Id);
                
                gigDetailsViewModel.isFollowing = isFollowing;
                gigDetailsViewModel.isAttending = isAttending;               

                
            }

            return View(gigDetailsViewModel);

        }
    }
}