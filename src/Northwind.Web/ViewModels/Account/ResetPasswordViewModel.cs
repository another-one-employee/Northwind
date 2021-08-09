using System.ComponentModel.DataAnnotations;

namespace Northwind.Web.ViewModels.Account
{
    public class ResetPasswordViewModel
    {
        [EmailAddress]
        public string Email { get; set; }
 
        [DataType(DataType.Password)]
        public string Password { get; set; }
 
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
 
        public string Code { get; set; }
    }
}
