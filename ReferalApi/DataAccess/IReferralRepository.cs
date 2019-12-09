using System.Collections.Generic;
using System.Data;
using codingchallenge.ReferalApi.Models;

namespace codingchallenge.ReferalApi.DataAccess
{
    public interface IReferralRepository
    {
        Referral GetReferral(IDbConnection conn, string referralTitle);
        IEnumerable<Referral> GetReferrals(IDbConnection conn, int? page);
        string CreateReferral(IDbConnection conn, string referralTitle);
        void EditReferral(IDbConnection conn, Referral referral, Referral newReferral);
        void DeleteReferral(IDbConnection conn, string referralTitle);
        void DeleteAllReferrals(IDbConnection conn);
    }
}