using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Faucet.API.MailClients
{
    public interface IEmailClient
    {
        public void Send(string message);
    }

    public class EmailClient : IEmailClient
    {
        private const string Subject = "Total claimed BTC";

        private readonly EmailOptions _emailOptions;

        public EmailClient(IOptions<EmailOptions> options)
        {
            _emailOptions = options.Value;
        }

        public void Send(string message)
        {
            var mailMessage = new MimeMessage();

            mailMessage.From.Add(new MailboxAddress("Faucet App", _emailOptions.From));
            mailMessage.To.Add(new MailboxAddress("Faucet Admin", _emailOptions.To));
            mailMessage.Subject = Subject;
            mailMessage.Body = new TextPart("plain")
            {
                Text = message
            };

            using var smtpClient = new SmtpClient();
            smtpClient.Connect("smtp.gmail.com", 465, true);
            smtpClient.Authenticate(_emailOptions.From, _emailOptions.SenderPassword);
            smtpClient.Send(mailMessage);
            smtpClient.Disconnect(true);
        }
    }
}
