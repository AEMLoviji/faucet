using Microsoft.Extensions.Logging;
using Quartz;
using System.Threading.Tasks;

namespace Faucet.API.Jobs
{
    public class SendEmailToAdminJob : IJob
    {
        private readonly ILogger<SendEmailToAdminJob> _logger;
        public SendEmailToAdminJob(ILogger<SendEmailToAdminJob> logger)
        {
            _logger = logger;
        }

        public Task Execute(IJobExecutionContext context)
        {
            return Task.CompletedTask;
        }
    }
}
