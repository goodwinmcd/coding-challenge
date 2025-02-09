using System.Collections.Generic;
using codingchallenge.ReferalApi.Models;

namespace codingchallenge.ReferalApi.Services
{
    public interface IReferralCrudService
    {
        Referral GetReferral(string referralTitle);
        IEnumerable<Referral> GetReferrals(int? page = null);
        string CreateReferral(string referralTitle);
        void EditReferral(Referral referral, Referral newReferral);
        void DeleteReferral(string referralTitle);
        void DeleteAllReferrals();
    }
}