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

            _referralCrudService.IncrementReferral(referral);
            return NoContent();
        }

        private Referral GetExistingReferral(string referralTitle) =>
            _referralCrudService.GetReferral(referralTitle);
    }
}
