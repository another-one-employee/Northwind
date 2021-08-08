using FluentValidation;

namespace Northwind.Web.ViewModels.Account
{
    public class RegisterViewModelValidator : AbstractValidator<RegisterViewModel>
    {
        public RegisterViewModelValidator()
        {
            RuleFor(rvm => rvm.Email)
                .NotNull();

            RuleFor(rvm => rvm.Password)
                .NotNull();

            RuleFor(rvm => rvm.PasswordConfirm)
                .NotNull()
                .Equal(rvm => rvm.Password)
                .WithMessage("Password mismatch");
        }
    }
}
