using codingchallenge.ReferalApi.Models;
using codingchallenge.ReferalApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReferalApi.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ReferralApiTests
{
    [TestClass]
    public class ReferralsControllerTest
    {
        private Mock<IReferralCrudService> _mockReferralCrudService;
        private ReferralsController _classUnderTest;

        [TestInitialize]
        public void Setup()
        {
            _mockReferralCrudService = new Mock<IReferralCrudService>();
            _classUnderTest = new ReferralsController(_mockReferralCrudService.Object);
        }

        [TestMethod]
        public void GetBadReferral_ShouldReturn404()
        {
            _mockReferralCrudService.Setup(m => m.GetReferral("fake"))
                .Returns<string>(null);

            var referral = _classUnderTest.GetReferral("fake");
            var statusCode = (NotFoundObjectResult)referral.Result;

            Assert.AreEqual(statusCode.StatusCode, (int)HttpStatusCode.NotFound);
        }

        [TestMethod]
        public void GetReferral_ShouldReturn200()
        {
            var referralValue = "dummy";
            var referral = new Referral
            {
                Title = referralValue,
                ReferralCount = 0
            };

            _mockReferralCrudService.Setup(m => m.GetReferral(referralValue))
                .Returns(referral);

            var returnedReferral = _classUnderTest.GetReferral(referralValue);
            var statusCode = returnedReferral.Result as OkObjectResult;
            var finalResult = (Referral)statusCode.Value;

            Assert.AreEqual(finalResult.Title, referralValue);
        }

        [TestMethod]
        public void GetPagenatedReferralsWithArg_ReturnsAListOfReferrals()
        {
            var referralList = BuildReferralList();

            _mockReferralCrudService.Setup(m => m.GetReferrals(1))
                .Returns(referralList);

            var returnedReferral = _classUnderTest.GetReferrals(1);
            var statusCode = returnedReferral.Result as OkObjectResult;
            var finalResult = (List<Referral>)statusCode.Value;

            Assert.AreEqual(finalResult.Count, referralList.Count());
        }

        [TestMethod]
        public void GetAllReferralsWithNoArgument_ReturnsAListOfReferrals()
        {
            var referralList = BuildReferralList();
            _mockReferralCrudService.Setup(m => m.GetReferrals(It.IsAny<int>()))
                .Returns(referralList);

            var returnedReferral = _classUnderTest.GetReferrals();
            var statusCode = returnedReferral.Result as OkObjectResult;
            var finalResult = (List<Referral>)statusCode.Value;

            Assert.AreEqual(finalResult.Count, referralList.Count());
        }

        [TestMethod]
        public void GetAllReferralsWithNoReferrals_ReturnsEmptyList()
        {
            _mockReferralCrudService.Setup(m => m.GetReferrals(1))
                .Returns<List<Referral>>(null);

            var returnedReferral = _classUnderTest.GetReferrals();
            var statusCode = returnedReferral.Result as OkObjectResult;
            var finalResult = (List<Referral>)statusCode.Value;

            Assert.AreEqual(finalResult.Count, 0);
        }

        [TestMethod]
        public void CreatingNewReferral_ShouldReturnCreatedStatusCode()
        {
            var referralTitle = "dummy";

            _mockReferralCrudService.Setup(m => m.GetReferral(referralTitle))
                .Returns<string>(null);

            _mockReferralCrudService.Setup(m => m.CreateReferral(referralTitle))
                .Returns(referralTitle);

            var result = _classUnderTest.PostReferral(referralTitle);

            var statusCode = result.Result as CreatedResult;
            Assert.AreEqual(statusCode.StatusCode, (int)HttpStatusCode.Created);
        }

        [TestMethod]
        public void CreatingNewReferralThatExist_ShouldCreateConflict()
        {
            var referralTitle = "dummy";
            var existingReferral = new Referral
            {
                Title = referralTitle,
                ReferralCount = 0,
            };

            _mockReferralCrudService.Setup(m => m.GetReferral(referralTitle))
                .Returns(existingReferral);

            var result = _classUnderTest.PostReferral(referralTitle);

            var statusCode = result.Result as ConflictObjectResult;
            Assert.AreEqual(statusCode.StatusCode, (int)HttpStatusCode.Conflict);
        }

        [TestMethod]
        public void IncrementingExistingReferral_ShouldReturnNoContent()
        {
            var referralTitle = "dummy";
            var existingReferral = new Referral
            {
                Title = referralTitle,
                ReferralCount = 0
            };

            _mockReferralCrudService.Setup(m => m.GetReferral(referralTitle))
                .Returns(existingReferral);

            var result = _classUnderTest.IncrementReferral(referralTitle);
            var statusCode = result as NoContentResult;
            Assert.AreEqual(statusCode.StatusCode, (int)HttpStatusCode.NoContent);
        }

        [TestMethod]
        public void IncrementingNonExistingReferral_ShouldReturnBadRequest()
        {
            var referralTitle = "dummy";

            _mockReferralCrudService.Setup(m => m.GetReferral(referralTitle))
                .Returns<Referral>(null);

            var result = _classUnderTest.IncrementReferral(referralTitle);
            var statusCode = result as BadRequestObjectResult;
            Assert.AreEqual(statusCode.StatusCode, (int)HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public void EditingTitleWithValidRequest_ShouldReturnNoContent()
        {
            var referralTitle = "dummy";
            var request = new EditTitleRequest
            {
                NewTitle = "new",
                Title = referralTitle,
            } ;
            var existingReferral = new Referral
            {
                Title = referralTitle,
                ReferralCount = 0
            };

            _mockReferralCrudService.Setup(m => m.GetReferral(referralTitle))
                .Returns(existingReferral);

            var result = _classUnderTest.EditReferralTitle(request);
            var statusCode = result as NoContentResult;
            Assert.AreEqual(statusCode.StatusCode, (int)HttpStatusCode.NoContent);
        }

        [TestMethod]
        public void EditingTitleWithInValidRequest_ShouldReturnBadRequest()
        {
            var referralTitle = "dummy";
            var request = new EditTitleRequest
            {
                Title = referralTitle,
            };

            var result = _classUnderTest.EditReferralTitle(request);
            var statusCode = result as BadRequestObjectResult;
            Assert.AreEqual(statusCode.StatusCode, (int)HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public void EditingTitleWithValidRequestButNonExistingReferral_ShouldReturnBadRequest()
        {
            var referralTitle = "dummy";
            var request = new EditTitleRequest
            {
                Title = referralTitle,
                NewTitle = "valid",
            };

            _mockReferralCrudService.Setup(m => m.GetReferral(referralTitle))
                .Returns<Referral>(null);

            var result = _classUnderTest.EditReferralTitle(request);
            var statusCode = result as BadRequestObjectResult;
            Assert.AreEqual(statusCode.StatusCode, (int)HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public void DeletingReferral_ShouldReturnNoContent()
        {
            var referralTitle = "dummy";
            var existingReferral = new Referral
            {
                Title = referralTitle,
                ReferralCount = 0
            };
            _mockReferralCrudService.Setup(m => m.GetReferral(referralTitle))
                .Returns(existingReferral);
            var result = _classUnderTest.DeleteReferral(referralTitle);
            var statusCode = result as NoContentResult;
            Assert.AreEqual(statusCode.StatusCode, (int)HttpStatusCode.NoContent);
        }

        [TestMethod]
        public void DeletingReferralOnNonExistingReferral_ShouldReturnBadRequest()
        {
            var referralTitle = "dummy";
            var result = _classUnderTest.DeleteReferral(referralTitle);
            var statusCode = result as BadRequestObjectResult;
            Assert.AreEqual(statusCode.StatusCode, (int)HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public void DeletingAllReferrals_ShouldReturnNoContent()
        {
            var result = _classUnderTest.DeleteAllReferrals();
            var statusCode = result as NoContentResult;
            Assert.AreEqual(statusCode.StatusCode, (int)HttpStatusCode.NoContent);
        }


        private IEnumerable<Referral> BuildReferralList()
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
            return referralList;
        }
    }
}
