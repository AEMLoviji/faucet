namespace Faucet.API.Data.Repositories
{
    public interface IUserRepository
    {
        public User Get(string email);

        public void Add(User user);
    }
}
