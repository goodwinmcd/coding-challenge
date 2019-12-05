using System.Data;
using codingchallenge.ReferalApi.Models;
using Microsoft.Extensions.Logging;
using Dapper;
using System.Linq;

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
            var sql = "SELECT Title FROM referrals WHERE Title=@referralTitle";
            var result = conn.Query<Referral>(sql, new { ReferralTitle = referralTitle });
            return result.FirstOrDefault();
        }

        public string CreateReferral(IDbConnection conn, string referralTitle)
        {
            var newReferral = new Referral
            {
                Title = referralTitle,
                Count = 0,
            };
            var sql = @"INSERT INTO referrals
                (Title,
                ReferralCount)
                VALUES
                (@Title,
                @Count)";

            _logger.LogInformation($"Adding new Referral {referralTitle}");
            conn.Execute(sql, newReferral);
            return referralTitle;
        }
    }
}