using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        [HttpGet("{referralName}")]
        public ActionResult<string> GetReferral(string referralTitle)
        {
            var referral = _referralCrudService.GetReferral(referralTitle);

            if (referral != null)
                return Ok(new { Referral = referral });
            else
                return NotFound("Referral does not exist");
        }

        /// <summary>
        /// This endpoint creates a referral in the db
        /// </summary>
        [HttpPost("{referralName}")]
        public ActionResult<string> PostReferral(string referralTitle)
        {
            var referral = _referralCrudService.GetReferral(referralTitle);
            if (referral != null)
                return Conflict("That referral already exist");

            var createdReferral = _referralCrudService.CreateReferral(referralTitle);
            return Created(createdReferral, new { Title = createdReferral, Count = 0 });
        }
    }
}
