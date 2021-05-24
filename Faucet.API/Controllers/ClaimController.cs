using Faucet.API.Data;
using Faucet.API.Data.Repositories;
using Faucet.API.RateServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Faucet.API.Controllers
{
    [ApiController]
    [Route("api/claim")]
    public class ClaimController : ControllerBase
    {
        private const decimal BitcointAmountPerRequest = 0.0001m;

        private readonly ILogger<ClaimController> _logger;
        private readonly IBalanceRepository _balanceRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBlockchainRateService _blockchainRateService;

        private decimal AvailableBitcoinsAmount => _balanceRepository.Get().BitcoinsAmount;

        private async Task<decimal> BtcToUsdRate() => await _blockchainRateService.GetUsdRateAsync();

        public ClaimController
        (
            ILogger<ClaimController> logger,
            IBalanceRepository balanceRepository,
            IUserRepository userRepository,
            ITransactionRepository transactionRepository,
            IUnitOfWork unitOfWork,
            IBlockchainRateService blockchainRateService
        )
        {
            _logger = logger;

            _balanceRepository = balanceRepository;
            _userRepository = userRepository;
            _transactionRepository = transactionRepository;

            _unitOfWork = unitOfWork;

            _blockchainRateService = blockchainRateService;
        }

        /// <summary>
        ///     Claim BTC to user by a given email.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/claim?email=:email
        ///
        /// </remarks>
        /// <param name="email">The email</param>
        /// <response code="400">If email is not provided</response>         
        /// <response code="403">If system does not have required amount BTC</response>
        /// <response code="429">If user tries to buy BTC more than once within 24h</response>
        /// <response code="200">If user claimed BTC</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Claim(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest();
            }

            _logger.LogInformation($"User '{email}' requested BTC");

            if (AvailableBitcoinsAmount < BitcointAmountPerRequest)
            {
                _logger.LogInformation($"System does not have the required amount. Available BTC amount is: {AvailableBitcoinsAmount}");
                return StatusCode((int)HttpStatusCode.Forbidden, "System does not have the required amount");
            }

            var user = _userRepository.Get(email) ?? RegisterNewUser(email);

            if (_transactionRepository.AnyTransactionInLast24Hours(user.Id))
            {
                _logger.LogInformation($"User '{user.Id}' tried to buy BTC more than once within 24h");

                return StatusCode((int)HttpStatusCode.TooManyRequests, "Each email can only claim once per 24h");
            }

            await ProcessClaimOrder(user.Id);

            return Ok();
        }

        private async Task ProcessClaimOrder(Guid userId)
        {
            var usdRate = await BtcToUsdRate();

            var newTransaction = Transaction.New
            (
                userId: userId,
                amount: BitcointAmountPerRequest,
                amountInUsd: ExchangeUtils.BtcToUsd(BitcointAmountPerRequest, usdRate)
            );

            _transactionRepository.Add(newTransaction);

            _balanceRepository.Change(-1 * BitcointAmountPerRequest);

            await _unitOfWork.Commit();

            _logger.LogInformation($"User '{userId}' claimed {BitcointAmountPerRequest} BTC paying {newTransaction.AmountInUsd} USD");
        }

        private User RegisterNewUser(string email)
        {
            var user = Data.User.New(email);

            _userRepository.Add(user);

            _logger.LogInformation($"User registered by email '{email}'");

            return user;
        }
    }
}
