using Faucet.API.Data;
using Faucet.API.Data.Repositories;
using Faucet.API.RateServices;
using Microsoft.Extensions.Logging;
using Quartz;
using System.Threading.Tasks;

namespace Faucet.API.Jobs
{
    public class BitcoinGrabberJob : IJob
    {
        private const int BuyBitcoinInWorth = 500; //USD

        private readonly ILogger<BitcoinGrabberJob> _logger;
        private readonly IBlockchainRateService _blockchainRateService;
        private readonly IBalanceRepository _balanceRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BitcoinGrabberJob
        (
            ILogger<BitcoinGrabberJob> logger,
            IBlockchainRateService blockchainRateService,
            IBalanceRepository balanceRepository,
            IUnitOfWork unitOfWork
        )
        {
            _logger = logger;
            _blockchainRateService = blockchainRateService;
            _balanceRepository = balanceRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var usdRate = await _blockchainRateService.GetUsdRateAsync();

            var bitcoinAmountToAdd = decimal.Round(BuyBitcoinInWorth / usdRate, 2);

            await AddNewBitcoins(bitcoinAmountToAdd);

            _logger.LogInformation($"{bitcoinAmountToAdd} BTC swapped from {BuyBitcoinInWorth} USD");
        }

        private async Task AddNewBitcoins(decimal bitcoinAmountToAdd)
        {
            _balanceRepository.Change(bitcoinAmountToAdd);

            await _unitOfWork.Commit();
        }
    }
}
