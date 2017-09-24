using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using GigHub.Controllers;
using GigHub.Core.Models;
using GigHub.Core.ViewModels;
using GigHub.IntegrationTests.Extensions;
using GigHub.Persistence;
using NUnit.Framework;

namespace GigHub.IntegrationTests.Controllers
{
    [TestFixture]
    public class GigsControllerTests
    {
        private GigsController _gigsController;
        
        private ApplicationDbContext _context;

        [SetUp]
        public void SetUp()
        {
            _context = new ApplicationDbContext();
            _gigsController = new GigsController(new UnitOfWork(_context));
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }


        [Test, Isolated]
        public void Mine_WhenCalled_ShouldReturnUpComingGigs()
        {
            //Arrange

            var user = _context.Users.First();
            _gigsController.MockCurrentUser(user.Id, user.UserName);

            var gig = new Gig()
            {
                Artist = user,
                DateTime = DateTime.Now.AddDays(1),
                Genre = _context.Genres.First(),
                Venue = "-"
            };

            _context.Gigs.Add(gig);
            _context.SaveChanges();

            // Action

            var result = _gigsController.Mine();

            // assert
            (result.ViewData.Model as IEnumerable<Gig>).Should().HaveCount(1);
        }

        [Test, Isolated]
        public void Update_WhenCalled_ShouldUpdateGivenGig()
        {
            //Arrange

            var user = _context.Users.First();
            _gigsController.MockCurrentUser(user.Id, user.UserName);

            var genre = _context.Genres.First(g=> g.Id==1);
            var gig = new Gig()
            {
                Artist = user,
                DateTime = DateTime.Now.AddDays(1),
                Genre = genre,
                Venue = "-"
            };

            _context.Gigs.Add(gig);
            _context.SaveChanges();

            // Action

            var result = _gigsController.Update(new GigFormViewModel()
            {
                Id = gig.Id,
                Date = DateTime.Today.AddMonths(1).ToString("d MMM yyyy"),
                Time="20:00",
                Genre = 2,
                Venue = "Venue"
            });

            // assert
            _context.Entry(gig).Reload();
            gig.DateTime.Should().Be(DateTime.Today.AddMonths(1).AddHours(20));
            gig.Venue.Should().Be("Venue");
            gig.GenreId.Should().Be(2);
        }
    }
}
