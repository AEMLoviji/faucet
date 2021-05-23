using Faucet.API.Data.Repositories;
using Faucet.API.Model;
using Faucet.API.RateServices;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Faucet.API.Controllers
{
    [ApiController]
    [Route("api/balance")]
    public class BalanceController : ControllerBase
    {
        private readonly IBalanceRepository _balanceRepository;
        private readonly IBlockchainRateService _blockchainRateService;

        public BalanceController
        (
            IBalanceRepository balanceRepository,
            IBlockchainRateService blockchainRateService
        )
        {
            _balanceRepository = balanceRepository;
            _blockchainRateService = blockchainRateService;
        }

        [HttpGet]
        public async Task<Balance> Get()
        {
            var bitcoinsAmount = _balanceRepository.Get().BitcoinsAmount;

            var usdRate = await _blockchainRateService.GetUsdRateAsync();

            return new Balance()
            {
                Btc = bitcoinsAmount,
                InUsd = ExchangeUtils.BtcToUsd(bitcoinsAmount, usdRate)
            };
        }
    }
}
