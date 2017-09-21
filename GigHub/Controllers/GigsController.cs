using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using GigHub.Core;
using GigHub.Core.Models;
using GigHub.Core.ViewModels;
using GigHub.Persistence;
using GigHub.Repositories;

namespace GigHub.Controllers
{
    public class GigsController : Controller
    {
        
        private readonly string _userId;
        
        private readonly IUnitOfWork _unitOfWork;

        public GigsController(IUnitOfWork unitOfWork)
        {
            
            _userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            _unitOfWork = unitOfWork;
        }


        [Authorize]
        public ActionResult Mine()
        {

            var gigs = _unitOfWork.Gigs.GetMyGigs(_userId, this);

            return View(gigs);
        }


        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new GigFormViewModel
            {
                Genres = _unitOfWork.Genres.GetGenres(),
                Heading = "Add a Gig"
            };

            return View("GigForm", viewModel);
        }
        [Authorize]
        public ActionResult Edit(int id)
        {
            var gig = _unitOfWork.Gigs.GetGigByIdForCurrentArtist(id, _userId);

            if (gig == null)
                return HttpNotFound();

            if(gig.ArtistId != _userId)
                return new HttpUnauthorizedResult();

            var viewModel = new GigFormViewModel
            {
                Id = gig.Id,
                Genres = _unitOfWork.Genres.GetGenres(),
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
                viewModel.Genres = _unitOfWork.Genres.GetGenres();
                return View("GigForm", viewModel);
            }

            var gig = new Gig()
            {
                ArtistId = _userId,
                DateTime = viewModel.GetDateTime(),
                GenreId = viewModel.Genre,
                Venue = viewModel.Venue
            };

            _unitOfWork.Gigs.Add(gig);
            _unitOfWork.Complete();

            return RedirectToAction("Mine", "Gigs");
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(GigFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = _unitOfWork.Genres.GetGenres();
                return View("GigForm", viewModel);
            }

            var gig = _unitOfWork.Gigs.GetGigWithAttendees(viewModel.Id);

            if (gig == null)
                return HttpNotFound();

            if(gig.ArtistId != _userId)
                return new HttpUnauthorizedResult();

            gig.Modify(viewModel.GetDateTime(), viewModel.Venue, viewModel.Genre);

            _unitOfWork.Complete();
            return RedirectToAction("Mine", "Gigs");
        }

        public ActionResult Attending()
        {
            var gigsViewModel = new GigsViewModel()
            {
                UpcomingGigs = _unitOfWork.Gigs.GetGigsUserAttending(_userId),
                ShowAction = User.Identity.IsAuthenticated,
                Heading = "Gigs I'm going",
                Attendances = _unitOfWork.Attendances.GetFutureAttendances(_userId).ToLookup(a => a.GigId)
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
            var gig = _unitOfWork.Gigs.GetGigWithArtist(id);

            if (gig == null) return HttpNotFound();

            var gigDetailsViewModel = new GigsDetailsViewModel()
            {
                Gig = gig
            };

            if (User.Identity.IsAuthenticated)
            {
                var isFollowing = _unitOfWork.Followings.GetFollower(_userId, gig.ArtistId) != null;
                var isAttending = _unitOfWork.Attendances.GetAttendance(gig.Id, _userId) != null;
                
                gigDetailsViewModel.isFollowing = isFollowing;
                gigDetailsViewModel.isAttending = isAttending;               

                
            }

            return View(gigDetailsViewModel);

        }
    }
}