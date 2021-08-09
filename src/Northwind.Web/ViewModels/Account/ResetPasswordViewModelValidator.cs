using FluentValidation;

namespace Northwind.Web.ViewModels.Account
{
    public class ResetPasswordViewModelValidator : AbstractValidator<ResetPasswordViewModel>
    {
        public ResetPasswordViewModelValidator()
        {
            RuleFor(rvm => rvm.Email)
                .NotNull();

            RuleFor(rvm => rvm.Password)
                .NotNull()
                .MinimumLength(6)
                .MaximumLength(100);

            RuleFor(rvm => rvm.ConfirmPassword)
                .NotNull()
                .Equal(rvm => rvm.Password)
                .WithMessage("Password mismatch");
        }
    }
}
