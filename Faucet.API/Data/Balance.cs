using System;

namespace Faucet.API.Data
{
    public class Balance
    {
        // Keyless entity is not supported in SQLite
        public int Id { get; set; }

        public decimal BitcoinsAmount { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
