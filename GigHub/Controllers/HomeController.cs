using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using GigHub.Core.Models;
using GigHub.Core.ViewModels;
using GigHub.Persistence;
using GigHub.Persistence.Repositories;
using Microsoft.AspNet.Identity;

namespace GigHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly GigsRepository _gigsRepository;
        private readonly AttendanceRepository attendanceRepository;
        private string _userId;

        public HomeController()
        {
            _dbContext = new ApplicationDbContext();
            _userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            _gigsRepository=new GigsRepository(_dbContext);
            attendanceRepository = new AttendanceRepository(_dbContext);
        }

        public ActionResult Index(string query = null)
        {
            var upcomingGigs = _dbContext.Gigs
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .Where(g => g.DateTime > DateTime.Now && !g.IsCanceled);

            if (!string.IsNullOrWhiteSpace(query))
            {
                upcomingGigs = upcomingGigs.Where(g =>
                    g.Artist.Name.Contains(query) || g.Venue.Contains(query) || g.Genre.Name.Contains(query));
            }

            var attendances = attendanceRepository.GetFutureAttendances(_userId).ToLookup(a => a.GigId);


            var viewModel = new GigsViewModel()
            {
                UpcomingGigs = upcomingGigs,
                ShowAction = User.Identity.IsAuthenticated,
                Heading = "Upcoming Gigs",
                SearchTerm = query,
                Attendances = attendances
            };

            return View("Gigs", viewModel);
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