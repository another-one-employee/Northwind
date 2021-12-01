using FluentValidation;

namespace Northwind.Web.ViewModels.Api.Products
{
    public class UpdateProductModelValidator : AbstractProductModelValidator<UpdateProductModel>
    {
        public UpdateProductModelValidator()
        {
            RuleFor(cpm => cpm.ProductID)
                .NotNull()
                .GreaterThan(0);
        }
    }
}
