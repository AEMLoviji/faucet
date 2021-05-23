namespace Faucet.API.Data.Repositories
{
    public interface IBalanceRepository
    {
        public Balance Get();

        public Balance Change(decimal amount);
    }
}
