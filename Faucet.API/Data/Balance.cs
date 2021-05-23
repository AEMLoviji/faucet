using System;

namespace Faucet.API.Data
{
    public class Balance
    {
        // Keyless entity is not supported in SQLite
        public int Id { get; set; }

        public decimal BitcoinsCount { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
