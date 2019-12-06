using System.ComponentModel.DataAnnotations;

namespace codingchallenge.ReferalApi.Models
{
    public class Referral
    {
        [Required]
        public string Title { get; set; }
        public int ReferralCount { get; set; }
    }
}