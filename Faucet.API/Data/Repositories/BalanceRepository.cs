using System;
using System.Linq;

namespace Faucet.API.Data.Repositories
{
    public class BalanceRepository : IBalanceRepository
    {
        private readonly FaucetDbContext _faucetDbContext;

        public BalanceRepository(FaucetDbContext faucetDbContext)
        {
            _faucetDbContext = faucetDbContext;
        }

        public Balance Get() => _faucetDbContext.Balance.Single();

        public Balance Change(decimal amount)
        {
            var currentBalance = Get();

            currentBalance.BitcoinsAmount += amount;
            currentBalance.UpdatedAt = DateTime.UtcNow;

            _faucetDbContext.Balance.Update(currentBalance);

            return currentBalance;
        }
    }
}
