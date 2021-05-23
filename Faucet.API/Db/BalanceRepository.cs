using Faucet.API.Data;
using System.Linq;

namespace Faucet.API.Db
{
    public class BalanceRepository : IBalanceRepository
    {
        private readonly FaucetDbContext _faucetDbContext;

        public BalanceRepository(FaucetDbContext faucetDbContext)
        {
            _faucetDbContext = faucetDbContext;
        }

        public Balance Get() => _faucetDbContext.Balance.Single();
    }
}
