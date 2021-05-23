using System;
using System.Linq;

namespace Faucet.API.Data.Repositories
{
    public class AdminEmailRepository : IAdminEmailRepository
    {
        private readonly FaucetDbContext _faucetDbContext;

        public AdminEmailRepository(FaucetDbContext faucetDbContext)
        {
            _faucetDbContext = faucetDbContext;
        }

        public DateTime LastSentTransactionDate()
        {
            return _faucetDbContext.AdminEmail.Any()
                ? _faucetDbContext.AdminEmail.Max(_ => _.LastSentTransactionDate)
                : DateTime.MinValue;
        }

        public void Add(AdminEmail adminEmail)
        {
            _faucetDbContext.AdminEmail.Add(adminEmail);
        }
    }
}
