using codingchallenge.ReferalApi.Models;

namespace codingchallenge.ReferalApi.Services
{
    public interface IReferralCrudService
    {
        Referral GetReferral(string referralTitle);
        string CreateReferral(string referralTitle);
        void IncrementReferral(Referral referral);
    }
}