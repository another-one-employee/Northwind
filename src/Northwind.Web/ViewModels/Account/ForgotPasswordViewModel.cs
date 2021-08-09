using System.ComponentModel.DataAnnotations;

namespace Northwind.Web.ViewModels.Account
{
    public class ForgotPasswordViewModel
    {
        [EmailAddress]
        public string Email { get; set; }
    }
}
