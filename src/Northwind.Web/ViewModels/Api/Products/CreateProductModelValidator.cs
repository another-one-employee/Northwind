using FluentValidation;

namespace Northwind.Web.ViewModels.Api.Products
{
    public class CreateProductModelValidator : AbstractValidator<CreateProductModel>
    {
        public CreateProductModelValidator()
        {
            RuleFor(cpm => cpm.ProductName)
                .NotNull()
                .MaximumLength(50);

            RuleFor(cpm => cpm.QuantityPerUnit)
                .MaximumLength(20);

            RuleFor(cpm => cpm.UnitPrice)
                .InclusiveBetween(0, (decimal) 922337203685477.5807);

            RuleFor(cpm => cpm.UnitsInStock)
                .InclusiveBetween((short)0, short.MaxValue)
                .WithMessage("{PropertyName} must be positive");

            RuleFor(cpm => cpm.UnitsOnOrder)
                .InclusiveBetween((short)0, short.MaxValue)
                .WithMessage("{PropertyName} must be positive");

            RuleFor(cpm => cpm.ReorderLevel)
                .InclusiveBetween((short)0, short.MaxValue)
                .WithMessage("{PropertyName} must be positive");

            RuleFor(cpm => cpm.Discontinued)
                .NotNull();
        }
    }
}
