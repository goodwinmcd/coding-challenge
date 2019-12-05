using System.Data;
using codingchallenge.ReferalApi.Models;

namespace codingchallenge.ReferalApi.DataAccess
{
    public interface IReferralRepository
    {
        Referral GetReferral(IDbConnection conn, string referralTitle);
        string CreateReferral(IDbConnection conn, string referralTitle);
        void IncrementReferral(IDbConnection conn, Referral referral);
    }
}