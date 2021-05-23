using System;
using System.Linq;

namespace Faucet.API.Data.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly FaucetDbContext _faucetDbContext;

        public TransactionRepository(FaucetDbContext faucetDbContext)
        {
            _faucetDbContext = faucetDbContext;
        }

        public bool AnyTransactionInLast24Hours(Guid userId) =>
            _faucetDbContext.Transaction.Any(_ =>
                _.UserId == userId &&
                _.RequestedAt >= DateTime.UtcNow.AddHours(-24)
            );

        public void Add(Transaction transaction)
        {
            _faucetDbContext.Transaction.Add(transaction);
        }
    }
}
