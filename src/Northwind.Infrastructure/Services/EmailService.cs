using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Northwind.Application.Interfaces;
using System.Threading.Tasks;

namespace Northwind.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private const string EmailSection = "Email";
        private const string PasswordSection = "Password";
        private const string MailBoxAddressName = "Northwind administration";
        private const string MailServiceHost = "smtp.gmail.com";
        private const int MailServicePort = 456;


        private readonly string _email;
        private readonly string _password;

        public EmailService(IConfiguration configuration)
        {
            var emailServiceSettings = configuration.GetSection(nameof(EmailService));

            _email = emailServiceSettings.GetSection(EmailSection).Value;
            _password = emailServiceSettings.GetSection(PasswordSection).Value;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();
 
            emailMessage.From.Add(new MailboxAddress(MailBoxAddressName, _email));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using var client = new SmtpClient();
            await client.ConnectAsync(MailServiceHost, MailServicePort, true);
            await client.AuthenticateAsync(_email, _password);

            await client.SendAsync(emailMessage);

            await client.DisconnectAsync(true);
        }
    }
}
