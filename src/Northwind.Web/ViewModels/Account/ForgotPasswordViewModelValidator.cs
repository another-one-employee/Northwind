using FluentValidation;

namespace Northwind.Web.ViewModels.Account
{
    public class ForgotPasswordViewModelValidator : AbstractValidator<ForgotPasswordViewModel>
    {
        public ForgotPasswordViewModelValidator()
        {
            RuleFor(fvm => fvm.Email)
                .NotNull()
                .EmailAddress();
        }
    }
}
