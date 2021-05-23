using System;

namespace Faucet.API.Data.Repositories
{
    public interface IAdminEmailRepository
    {
        public DateTime LastSentTransactionDate();

        public void Add(AdminEmail adminEmail);
    }
}
