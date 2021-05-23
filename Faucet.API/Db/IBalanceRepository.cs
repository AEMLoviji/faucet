using Faucet.API.Data;

namespace Faucet.API.Db
{
    public interface IBalanceRepository
    {
        public Balance Get();
    }
}
