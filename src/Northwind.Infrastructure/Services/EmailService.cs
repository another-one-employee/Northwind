using MailKit.Net.Smtp;
using MimeKit;
using Northwind.Application.Interfaces;
using System.Threading.Tasks;

namespace Northwind.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        // TODO: set email and pass (2021/08/09 10:08 PM)
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();
 
            emailMessage.From.Add(new MailboxAddress("Northwind administration", "set email"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using var client = new SmtpClient();
            await client.ConnectAsync("smtp.gmail.com", 465, true);
            await client.AuthenticateAsync("set email", "set pass");

            await client.SendAsync(emailMessage);

            await client.DisconnectAsync(true);
        }
    }
}
