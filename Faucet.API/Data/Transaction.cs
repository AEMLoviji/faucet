using System;

namespace Faucet.API.Data
{
    public class Transaction
    {
        public Guid Id { get; set; }

        public decimal Amount { get; set; }

        public decimal AmountInUsd { get; set; }

        public Guid UserId { get; set; }

        public DateTime RequestedAt { get; set; }

        public static Transaction New(Guid userId, decimal amount, decimal amountInUsd) => new Transaction
        {
            Id = Guid.NewGuid(),
            Amount = amount,
            AmountInUsd = amountInUsd,
            UserId = userId,
            RequestedAt = DateTime.UtcNow
        };
    }
}
