namespace Faucet.API.Data.Repositories
{
    public interface IBalanceRepository
    {
        public Balance Get();

        /// <summary>
        /// increase or decrease BTC balance
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public Balance Change(decimal amount);
    }
}
