using System;

namespace Faucet.API.Data
{
    public class User
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public DateTime RegisteredAt { get; set; }

        public static User New(string email) => new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            RegisteredAt = DateTime.UtcNow
        };
    }
}
