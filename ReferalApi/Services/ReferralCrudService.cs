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
                referral = _referralRepository.GetReferral(
                    _dbConnection,
                    referralTitle.ToLower());
            return referral;
        }

        public string CreateReferral(string referralTitle)
        {
            _dbConnection.Open();
            using (var transaction = _dbConnection.BeginTransaction())
                _referralRepository.CreateReferral(_dbConnection, referralTitle);

            return referralTitle;
        }

        public void IncrementReferral(Referral referral)
        {
            _dbConnection.Open();
            using (var transaction = _dbConnection.BeginTransaction())
                _referralRepository.IncrementReferral(_dbConnection, referral);
        }
    }
}