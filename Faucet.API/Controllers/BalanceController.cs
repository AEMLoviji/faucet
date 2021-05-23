using Faucet.API.Db;
using Faucet.API.Model;
using Faucet.API.RateServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Faucet.API.Controllers
{
    [ApiController]
    [Route("api/balance")]
    public class BalanceController : ControllerBase
    {
        private readonly ILogger<BalanceController> _logger;
        private readonly IBalanceRepository _balanceRepository;
        private readonly IBlockchainRateService _blockchainRateService;

        public BalanceController
        (
            ILogger<BalanceController> logger,
            IBalanceRepository balanceRepository,
            IBlockchainRateService blockchainRateService
        )
        {
            _logger = logger;
            _balanceRepository = balanceRepository;
            _blockchainRateService = blockchainRateService;
        }

        [HttpGet]
        public async Task<Balance> Get()
        {
            var bitcoinsCount = _balanceRepository.Get().BitcoinsCount;

            var usdRate = await _blockchainRateService.GetUsdRateAsync();

            return new Balance()
            {
                Btc = bitcoinsCount,
                Usd = bitcoinsCount * usdRate
            };
        }
    }
}
