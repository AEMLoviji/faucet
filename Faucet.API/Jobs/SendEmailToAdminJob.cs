using Faucet.API.Data;
using Faucet.API.Data.Repositories;
using Faucet.API.MailClients;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Threading.Tasks;

namespace Faucet.API.Jobs
{
    public class SendEmailToAdminJob : IJob
    {
        private readonly ILogger<SendEmailToAdminJob> _logger;
        private readonly IAdminEmailRepository _adminEmailRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailClient _emailClient;

        public SendEmailToAdminJob
        (
            ILogger<SendEmailToAdminJob> logger,
            IAdminEmailRepository adminEmailRepository,
            ITransactionRepository transactionRepository,
            IUnitOfWork unitOfWork,
            IEmailClient emailClient
        )
        {
            _logger = logger;
            _transactionRepository = transactionRepository;
            _adminEmailRepository = adminEmailRepository;
            _unitOfWork = unitOfWork;
            _emailClient = emailClient;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var lastEmailSentAt = _adminEmailRepository.LastSentTransactionDate();

            (DateTime lastClaimedAt, decimal totalClaimed) = _transactionRepository.GetAdminReportInfo(lastEmailSentAt);

            var message = $"Claimed {totalClaimed} BTC since last sent email";

            _emailClient.Send(message);

            _adminEmailRepository.Add(new AdminEmail { Id = Guid.NewGuid(), LastSentTransactionDate = lastClaimedAt });

            await _unitOfWork.Commit();

            _logger.LogInformation($"Email sent to admin with message: '{message}'");
        }
    }
}
