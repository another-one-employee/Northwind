using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Northwind.Application.Interfaces;
using System.Threading.Tasks;

namespace Northwind.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _email;
        private readonly string _password;

        public EmailService(IConfiguration configuration)
        {
            var emailServiceSettings = configuration.GetSection(nameof(EmailService));

            _email = emailServiceSettings.GetSection("Email").Value;
            _password = emailServiceSettings.GetSection("Password").Value;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();
 
            emailMessage.From.Add(new MailboxAddress("Northwind administration", _email));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using var client = new SmtpClient();
            await client.ConnectAsync("smtp.gmail.com", 465, true);
            await client.AuthenticateAsync(_email, _password);

            await client.SendAsync(emailMessage);

            await client.DisconnectAsync(true);
        }
    }
}
