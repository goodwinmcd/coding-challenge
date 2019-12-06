using System.Collections.Generic;
using System.Data;
using codingchallenge.ReferalApi.Models;

namespace codingchallenge.ReferalApi.DataAccess
{
    public interface IReferralRepository
    {
        Referral GetReferral(IDbConnection conn, string referralTitle);
        IEnumerable<Referral> GetReferrals(IDbConnection conn);
        string CreateReferral(IDbConnection conn, string referralTitle);
        void IncrementReferral(IDbConnection conn, Referral referral);
    }
}