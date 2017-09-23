using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using FluentAssertions;
using GigHub.Core.Models;
using GigHub.Persistence;
using GigHub.Persistence.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GigHub.Tests.Persistence.Repositories
{
    public static class MockDbSetExtensions
    {
        public static void SetSource<T>(this Mock<DbSet<T>> mockSet, List<T> source) where T : class
        {
            var data = source.AsQueryable();

            
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
        }
    }



    [TestClass]
    public class GigRepositoryTests
    {
        private GigsRepository _repository;
        private Mock<DbSet<Gig>> _mockGiggDbSet;


        [TestInitialize]
        public void TestInitiliaze()
        {
            _mockGiggDbSet = new Mock<DbSet<Gig>>();

            var mockDbContext = new Mock<IApplicationDbContext>();
            mockDbContext.SetupGet(m => m.Gigs).Returns(_mockGiggDbSet.Object);

            _repository = new GigsRepository(mockDbContext.Object);
        }

        [TestMethod]
        public void GetUpcomingGigsByArtist_GigIsInThePast_ShouldNotBeReturned()
        {
            var gig = new Gig() {DateTime = DateTime.Today.AddDays(-1), ArtistId = "1"};

            _mockGiggDbSet.SetSource(new List<Gig>() {gig});

            var result = _repository.GetUpcomingGigsByArtist("1");

            result.Should().BeEmpty();
        }

        [TestMethod]
        public void GetUpcomingGigsByArtist_GigIsCancelled_ShouldNotBeReturned()
        {
            var gig = new Gig() { DateTime = DateTime.Today.AddDays(2), ArtistId = "1" };

            gig.Cancel();

            _mockGiggDbSet.SetSource(new List<Gig>() { gig });

            var result = _repository.GetUpcomingGigsByArtist("1");

            result.Should().BeEmpty();
        }

        [TestMethod]
        public void GetUpcomingGigs_GigIsForADifferentArtist_ShouldNotBeReturned()
        {
            var gig = new Gig() { DateTime = DateTime.Today.AddDays(2), ArtistId = "2" };

            _mockGiggDbSet.SetSource(new List<Gig>() { gig });

            var result = _repository.GetUpcomingGigsByArtist("1");

            result.Should().BeEmpty();
        }

        [TestMethod]
        public void GetUpcomingGigs_GigIsForTheGivenArtistAndIsInTheFuture_ShouldBeReturned()
        {
            var gig = new Gig() { DateTime = DateTime.Today.AddDays(2), ArtistId = "1" };

            _mockGiggDbSet.SetSource(new List<Gig>() { gig });

            var result = _repository.GetUpcomingGigsByArtist("1");

            result.Should().Contain(gig);
        }
    }
}
