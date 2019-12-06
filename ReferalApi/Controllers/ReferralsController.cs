using System.Collections.Generic;
using codingchallenge.ReferalApi.Models;
using codingchallenge.ReferalApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ReferalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReferralsController : ControllerBase
    {
        private readonly IReferralCrudService _referralCrudService;
        public ReferralsController(IReferralCrudService referralCrudService)
        {
            _referralCrudService = referralCrudService;
        }

        /// <summary>
        /// Endpoint to retrieve a referral
        /// </summary>
        [HttpGet("{referralTitle}")]
        public ActionResult<Referral> GetReferral(string referralTitle)
        {
            var referral = _referralCrudService.GetReferral(referralTitle);

            if (referral != null)
                return Ok(referral);
            else
                return NotFound("Referral does not exist");
        }

        /// <summary>
        /// Endpoint to retrieve all referrals
        /// </summary>
        [HttpGet("all")]
        public ActionResult<IEnumerable<Referral>> GetReferrals()
        {
            var referrals = _referralCrudService.GetReferrals();
            referrals = referrals ?? new List<Referral>();
            return Ok(referrals);
        }

        /// <summary>
        /// This endpoint creates a referral in the db
        /// </summary>
        [HttpPost("{referralTitle}")]
        public ActionResult<string> PostReferral([FromRoute]string referralTitle)
        {
            var referral = _referralCrudService.GetReferral(referralTitle);
            if (referral != null)
                return Conflict("That referral already exist");

            var createdReferral = _referralCrudService.CreateReferral(referralTitle);
            return Created(createdReferral, new { Title = createdReferral, Count = 0 });
        }

        /// <summary>
        /// This endpoint increments a referral count in the db
        /// </summary>
        [HttpPut("{referralTitle}")]
        public ActionResult IncrementReferral(string referralTitle)
        {
            var referral = GetExistingReferral(referralTitle);
            if (referral == null)
                return BadRequest("That referral does not exist");

            var newReferral = new Referral
            {
                Title = referralTitle,
                ReferralCount = referral.ReferralCount + 1
            };
            _referralCrudService.EditReferral(referral, newReferral);
            return NoContent();
        }

        /// <summary>
        /// This endpoint changes title of a referral
        /// </summary>
        [HttpPut("editTitle")]
        public ActionResult EditReferralTitle([FromBody]EditTitleRequest editData)
        {
            if (editData.Validate())
                return BadRequest("You must provide the old title and new title");

            var referral = GetExistingReferral(editData.Title);
            if (referral == null)
                return BadRequest("The referral does not exist");

            var newReferral = new Referral
            {
                Title = editData.NewTitle,
                ReferralCount = referral.ReferralCount,
            };
            _referralCrudService.EditReferral(referral, newReferral);
            return NoContent();
        }

        /// <summary>
        /// This endpoint deletes a referral
        /// </summary>
        [HttpDelete("{referralTitle}")]
        public ActionResult DeleteReferral(string referralTitle)
        {
            if (GetExistingReferral(referralTitle) == null)
                return BadRequest("That referral does not exist");

            _referralCrudService.DeleteReferral(referralTitle);

            return NoContent();
        }

        private Referral GetExistingReferral(string referralTitle) =>
            _referralCrudService.GetReferral(referralTitle);
    }
}
