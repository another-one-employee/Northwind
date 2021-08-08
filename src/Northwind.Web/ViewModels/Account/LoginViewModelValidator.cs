using FluentValidation;

namespace Northwind.Web.ViewModels.Account
{
    public class LoginViewModelValidator : AbstractValidator<LoginViewModel>
    {
        public LoginViewModelValidator()
        {
            RuleFor(lvm => lvm.Email).NotNull();

            RuleFor(lvm => lvm.Password).NotNull();
        }
    }
}
