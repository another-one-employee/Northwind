using System.ComponentModel.DataAnnotations;

namespace Northwind.Web.ViewModels.Account
{
    public class LoginViewModel
    {
        public string Email { get; set; }
        
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string PasswordConfirm { get; set; }

        public bool RememberMe { get; set; }
         
        public string ReturnUrl { get; set; }
    }
}