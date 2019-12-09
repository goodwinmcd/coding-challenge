using System.Collections.Generic;
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
                referral = _referralRepository.GetReferral(
                    _dbConnection,
                    referralTitle.ToLower());
                transaction.Commit();
            }
            _dbConnection.Close();
            return referral;
        }

        public IEnumerable<Referral> GetReferrals(int? page = null)
        {
            IEnumerable<Referral> referrals;
            _dbConnection.Open();
            using (var transaction = _dbConnection.BeginTransaction())
            {
                referrals = _referralRepository.GetReferrals(_dbConnection, page);
                transaction.Commit();
            }
            _dbConnection.Close();
            return referrals;
        }

        public string CreateReferral(string referralTitle)
        {
            _dbConnection.Open();
            using (var transaction = _dbConnection.BeginTransaction())
            {
                _referralRepository.CreateReferral(
                    _dbConnection,
                    referralTitle.ToLower());
                transaction.Commit();
            }
            _dbConnection.Close();
            return referralTitle;
        }

        public void EditReferral(Referral referral, Referral newReferral)
        {
            _dbConnection.Open();
            using (var transaction = _dbConnection.BeginTransaction())
            {
                _referralRepository.EditReferral(_dbConnection, referral, newReferral);
                transaction.Commit();
            }
            _dbConnection.Close();
        }

        public void DeleteReferral(string referralTitle)
        {
            _dbConnection.Open();
            using (var transaction = _dbConnection.BeginTransaction())
            {
                _referralRepository.DeleteReferral(_dbConnection, referralTitle);
                transaction.Commit();
            }
            _dbConnection.Close();
        }
    }
}