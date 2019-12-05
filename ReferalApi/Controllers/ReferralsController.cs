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
    }
}
