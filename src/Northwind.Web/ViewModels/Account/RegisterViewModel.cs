using System.ComponentModel.DataAnnotations;

namespace Northwind.Web.ViewModels.Account
{
    public class RegisterViewModel
    {
        public string Email { get; set; }
        
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
    }
}
