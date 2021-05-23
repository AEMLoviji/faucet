using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Faucet.API.RateServices
{
    public interface IBlockchainRateService
    {
        public Task<decimal> GetUsdRateAsync();
    }

    public class BlockchainRateService : IBlockchainRateService
    {
        private const string ExchangeRateEndpoint = "ticker";

        public HttpClient Client { get; }

        public BlockchainRateService(HttpClient client)
        {
            Client = client;
        }
        public async Task<decimal> GetUsdRateAsync()
        {
            var response = await Client.GetStreamAsync(ExchangeRateEndpoint);

            using var jsonDocument = await JsonDocument.ParseAsync(response);

            // response sample
            //{
            //  "USD": {
            //      "15m": 33673.58,
            //      "last": 33673.58,
            //      "buy": 33673.58,
            //      "sell": 33673.58,
            //      "symbol": "$"
            //  }
            //}
            var usdElement = jsonDocument.RootElement.GetProperty("USD");

            return usdElement.GetProperty("last").GetDecimal();
        }
    }
}
