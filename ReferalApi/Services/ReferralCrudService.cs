using System;
using System.Data;
using codingchallenge.ReferalApi.DataAccess;
using codingchallenge.ReferalApi.Models;

namespace codingchallenge.ReferalApi.Services
{
    public class ReferralCrudService : IReferralCrudService
    {
        private readonly IDbConnection _dbConnection;
        private readonly IReferralRepository _referralRepository;

        public ReferralCrudService(
            IDbConnection dbConnection,
            IReferralRepository referralRepository)
        {
            _referralRepository = referralRepository;
            _dbConnection = dbConnection;
        }

        public Referral GetReferral(string referralTitle)
        {
            Referral referral;
            _dbConnection.Open();
            using (var transaction = _dbConnection.BeginTransaction())
            {
                referral = _referralRepository.GetReferral(_dbConnection, referralTitle);
            }
            return referral;
        }
    }
}