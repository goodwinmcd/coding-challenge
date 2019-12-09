using System.Collections.Generic;
using System.Data;
using System.Linq;
using codingchallenge.ReferalApi.DataAccess;
using codingchallenge.ReferalApi.Models;
using codingchallenge.ReferalApi.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace codingchallenge.ReferralApiTests.Services
{
    [TestClass]
    public class ReferralCrudServiceTests
    {
        private Mock<IDbConnection> _mockDbConnection;
        private Mock<IReferralRepository> _mockReferralRepository;
        private ReferralCrudService _classUnderTest;

        [TestInitialize]
        public void Setup()
        {
            _mockDbConnection = new Mock<IDbConnection>();
            _mockReferralRepository = new Mock<IReferralRepository>();
            var _mockDbTransaction = new Mock<IDbTransaction>();
            _mockDbConnection.Setup(m => m.BeginTransaction())
                .Returns(_mockDbTransaction.Object);
            _classUnderTest = new ReferralCrudService(
                _mockDbConnection.Object,
                _mockReferralRepository.Object);
        }

        [TestMethod]
        public void GetReferral_ShouldReturnReferral()
        {
            var title = "dummy";
            var returnReferral = new Referral
            {
                Title = title,
                ReferralCount = 0
            };

            _mockReferralRepository.Setup(m => m.GetReferral(_mockDbConnection.Object, title))
                .Returns(returnReferral);

            var returnedReferral = _classUnderTest.GetReferral(title);

            Assert.AreEqual(returnReferral.Title, title);
        }

        [TestMethod]
        public void GetReferrals_ShouldReturnMultipleReferrals()
        {
            var referral1 = new Referral
            {
                Title = "referral1",
                ReferralCount = 0,
            };
            var referral2 = new Referral
            {
                Title = "referral2",
                ReferralCount = 0,
            };
            var referral3 = new Referral
            {
                Title = "referral3",
                ReferralCount = 0,
            };

            var referralList = new List<Referral>
            {
                referral1,
                referral2,
                referral3,
            };

            _mockReferralRepository.Setup(m => m.GetReferrals(_mockDbConnection.Object, null))
                .Returns(referralList);

            var referrals = _classUnderTest.GetReferrals(null);

            Assert.AreEqual(referrals.Count(), referralList.Count());
        }

        [TestMethod]
        public void CreateReferral_ShouldReturnCreatedTitle()
        {
            var newTitle = "dummy";

            _mockReferralRepository.Setup(
                m => m.CreateReferral(_mockDbConnection.Object, newTitle))
                    .Returns(newTitle);

            var newReferralTitle = _classUnderTest.CreateReferral(newTitle);
            Assert.AreEqual(newReferralTitle, newTitle);
        }

        [TestMethod]
        public void EditReferral_ShouldCallRepoMethod()
        {
            var oldReferral = new Referral
            {
                Title = "old",
                ReferralCount = 0,
            };
            var newReferral = new Referral
            {
                Title = "new",
                ReferralCount = 0,
            };

            _classUnderTest.EditReferral(oldReferral, newReferral);

            _mockReferralRepository.Verify(
                m => m.EditReferral(
                    _mockDbConnection.Object,
                    oldReferral,
                    newReferral),
                Times.Once);
        }

        [TestMethod]
        public void DeleteReferral_ShouldCallRepoMethod()
        {
            var referralTitle = "dummy";
            _classUnderTest.DeleteReferral(referralTitle);

            _mockReferralRepository.Verify(
                m => m.DeleteReferral(_mockDbConnection.Object, referralTitle),
                Times.Once);
        }

        [TestMethod]
        public void DeleteAllReferral_ShouldCallRepoMethod()
        {
            _classUnderTest.DeleteAllReferrals();

            _mockReferralRepository.Verify(
                m => m.DeleteAllReferrals(_mockDbConnection.Object),
                Times.Once);
        }
    }
}