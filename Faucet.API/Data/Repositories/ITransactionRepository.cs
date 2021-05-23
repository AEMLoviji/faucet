using System;

namespace Faucet.API.Data.Repositories
{
    public interface ITransactionRepository
    {
        public bool AnyTransactionInLast24Hours(Guid userId);

        public void Add(Transaction transaction);

        public (DateTime lastClaimedAt, decimal totalClaimed) GetAdminReportInfo(DateTime date);
    }
}
