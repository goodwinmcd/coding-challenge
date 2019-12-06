using System.Collections.Generic;
using codingchallenge.ReferalApi.Models;

namespace codingchallenge.ReferalApi.Services
{
    public interface IReferralCrudService
    {
        Referral GetReferral(string referralTitle);
        IEnumerable<Referral> GetReferrals();
        string CreateReferral(string referralTitle);
        void EditReferral(Referral referral);
    }
}