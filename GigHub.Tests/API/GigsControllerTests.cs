using FluentAssertions;
using GigHub.API;
using GigHub.Core;
using GigHub.Core.Models;
using GigHub.Core.Repositories;
using GigHub.Tests.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace GigHub.Tests.API
{
    [TestClass]
    public class GigsControllerTests
    {
        private GigsController _controller;
        private Mock<IGigRepository> _mockRepository;
        private Mock<IUnitOfWork> _mockUoW;
        private string _userId = "1";

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new Mock<IGigRepository>();

            _mockUoW = new Mock<IUnitOfWork>();
            _mockUoW.SetupGet(u => u.Gigs).Returns(_mockRepository.Object);

            _controller = new GigsController(_mockUoW.Object);
            _controller.SetMockUser(_userId, "user1@domain.com");
        }

        [TestMethod]
        public void Cancel_NoGigWithGivenIdExists_ShouldReturnNotFound()
        {
            var cancelTask = _controller.Cancel(1);
            cancelTask.Wait();

            cancelTask.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void Cancel_GigIsCanceled_ShouldReturnNotFound()
        {

            var gig = new Gig();
            gig.Cancel();

            Task<Gig> t = Task.Factory.StartNew(() => { return gig; });
            _mockRepository.Setup(r => r.GetGigAsync(1)).Returns(t);

            var cancelTask = _controller.Cancel(1);
            cancelTask.Wait();

            cancelTask.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void Cancel_UserCancelingAnotherUsersGig_ShouldReturnUnauthorized()
        {
            var gig = new Gig { ArtistId = _userId + "-" };

            Task<Gig> t = Task.Factory.StartNew(() => { return gig; });
            _mockRepository.Setup(r => r.GetGigAsync(1)).Returns(t);

            var cancelTask = _controller.Cancel(1);
            cancelTask.Wait();

            cancelTask.Result.Should().BeOfType<UnauthorizedResult>();
        }

        [TestMethod]
        public void Cancel_ValidRequest_ShouldReturnOk()
        {
            var gig = new Gig { ArtistId = _userId };

            Task<Gig> gigTask = Task.Factory.StartNew(() => { return gig; });
            _mockRepository.Setup(r => r.GetGigAsync(1)).Returns(gigTask);

            Task<Tuple<int, string>> sucessTask = Task.Factory.StartNew(() => { return new Tuple<int, string>(0, "SUCCESS"); });
            _mockUoW.Setup(u => u.CompleteAsync()).Returns(sucessTask);

            var cancelTask = _controller.Cancel(1);
            cancelTask.Wait();

            cancelTask.Result.Should().BeOfType<OkResult>();
        }
    }
}