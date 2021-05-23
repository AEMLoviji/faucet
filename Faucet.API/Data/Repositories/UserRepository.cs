using System.Linq;

namespace Faucet.API.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly FaucetDbContext _faucetDbContext;

        public UserRepository(FaucetDbContext faucetDbContext)
        {
            _faucetDbContext = faucetDbContext;
        }

        public User Get(string email) => _faucetDbContext.User.SingleOrDefault(_ => _.Email == email);

        public void Add(User user) => _faucetDbContext.User.Add(user);
    }
}
