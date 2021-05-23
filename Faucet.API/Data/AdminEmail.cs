using System;

namespace Faucet.API.Data
{
    public class AdminEmail
    {
        public Guid Id { get; set; }

        public DateTime LastSentTransactionDate { get; set; }
    }
}
