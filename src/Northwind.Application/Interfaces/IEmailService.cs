using System.Threading.Tasks;

namespace Northwind.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
