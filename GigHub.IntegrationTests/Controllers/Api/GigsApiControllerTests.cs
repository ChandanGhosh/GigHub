using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Results;
using FluentAssertions;
using GigHub.Controllers.Api;
using GigHub.Core.Models;
using GigHub.IntegrationTests.Extensions.Api;
using GigHub.Persistence;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace GigHub.IntegrationTests.Controllers.Api
{
    /// <summary>
    /// Summary description for GigsApiControllerTests
    /// </summary>
    [TestFixture]
    public class GigsApiControllerTests
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
        public void Cancel_ShouldReturn_GigIsCanceled()
        {
            //Arrage
            var user = _context.Users.First();
            _gigsController.MockCurrentUser(user.Id, user.UserName);
            var genre = _context.Genres.First(g => g.Id == 1);
            var gig = new Gig()
            {
                Artist = user,
                DateTime = DateTime.Now.AddDays(1),
                Genre = genre,
                Venue = "Venue1"

            };

            _context.Gigs.Add(gig);
            _context.SaveChanges();

            // Action
            var result = _gigsController.Cancel(gig.Id);

            // assert
            _context.Entry(gig).Reload();
            gig.IsCanceled.Should().Be(true);
            result.Should().BeOfType<OkResult>();
        }
    }
}
