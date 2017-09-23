using System.Security.Claims;
using System.Security.Principal;
using System.Web.Http.Results;
using FluentAssertions;
using GigHub.Controllers.Api;
using GigHub.Core;
using GigHub.Core.Repositories;
using GigHub.Tests.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using GigHub.Core.Models;

namespace GigHub.Tests.Controllers.Api
{
    /// <summary>
    /// Summary description for GigsControllerTests
    /// </summary>
    [TestClass]
    public class GigsControllerTests
    {
        private GigsController _gigsController;
        private Mock<IGigsRepositories> _mockRepository;
        private string _userId;

        [TestInitialize]
        public void TestInitalize()
        {
            _mockRepository = new Mock<IGigsRepositories>();
            var mockUoW = new Mock<IUnitOfWork>();
            mockUoW.SetupGet(uow => uow.Gigs).Returns(_mockRepository.Object);


            _gigsController = new GigsController(mockUoW.Object);
            _userId = "1";
            _gigsController.MockCurrentUser(_userId, "user1@domain.com");

        }

        [TestMethod]
        public void Cancel_NoGigWithGivenIdExists_ShouldReturnNotFound()
        {
            var result = _gigsController.Cancel(1);

            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void Cancel_GigIsCanceled_ShouldReturnNotFound()
        {
            var gig = new Gig();
            gig.Cancel();

            _mockRepository.Setup(r => r.GetGigWithAttendees(1)).Returns(gig);

            var result = _gigsController.Cancel(1);

            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void Cancel_UserCancelingAnotherUsersGig_ShouldReturnUnAuthorized()
        {
            var gig = new Gig() {ArtistId = _userId + "-"};

            _mockRepository.Setup(r => r.GetGigWithAttendees(1)).Returns(gig);

            var result = _gigsController.Cancel(1);

            result.Should().BeOfType<UnauthorizedResult>();
        }

        [TestMethod]
        public void Cancel_ValidGigByArtist_ShouldReturnOk()
        {
            var gig = new Gig() {ArtistId = _userId};

            _mockRepository.Setup(r => r.GetGigWithAttendees(1)).Returns(gig);

            var result = _gigsController.Cancel(1);

            result.Should().BeOfType<OkResult>();
        }

    }
}
