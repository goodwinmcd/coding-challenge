using System.Data;
using codingchallenge.ReferalApi.Models;
using Microsoft.Extensions.Logging;
using Dapper;
using System.Linq;
using System.Collections.Generic;

namespace codingchallenge.ReferalApi.DataAccess
{
    public class ReferralRepository : IReferralRepository
    {
        private readonly ILogger<ReferralRepository> _logger;

        public ReferralRepository(ILogger<ReferralRepository> logger)
        {
            _logger = logger;
        }

        public Referral GetReferral(IDbConnection conn, string referralTitle)
        {
            _logger.LogInformation($"Checking database for referral {referralTitle}");
            var sql = "SELECT title, referralCount FROM referrals WHERE Title=@referralTitle";
            var result = conn.Query<Referral>(sql, new { ReferralTitle = referralTitle });
            return result.FirstOrDefault();
        }

        public IEnumerable<Referral> GetReferrals(IDbConnection conn)
        {
            _logger.LogInformation($"Retrieving all referrals");
            var sql = "SELECT title, referralCount FROM referrals";
            var result = conn.Query<Referral>(sql);
            return result;
        }

        public string CreateReferral(IDbConnection conn, string referralTitle)
        {
            var newReferral = new Referral
            {
                Title = referralTitle,
                ReferralCount = 0,
            };
            var sql = @"INSERT INTO referrals
                (title,
                referralCount)
                VALUES
                (@Title,
                @ReferralCount)";

            _logger.LogInformation($"Adding new Referral {referralTitle}");
            conn.Execute(sql, newReferral);
            return referralTitle;
        }

        public void EditReferral(IDbConnection conn, Referral referral, Referral newReferral)
        {
            _logger.LogInformation($"Editing referral {referral.Title}");
            var sql = @"UPDATE referrals
                SET referralCount = @ReferralCount, title = @Title
                WHERE title = @OldTitle";

            conn.Execute(
                sql,
                new {
                    ReferralCount = newReferral.ReferralCount,
                    Title = newReferral.Title,
                    OldTitle = referral.Title
                 });
        }

        public void DeleteReferral(IDbConnection conn, string referralTitle)
        {
            _logger.LogInformation($"Deleting referral {referralTitle}");

            var sql = @"DELETE FROM referrals WHERE title=@ReferralTitle";

            conn.Execute(sql, new { ReferralTitle = referralTitle });
        }
    }
}