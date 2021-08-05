using FluentValidation;

namespace Northwind.Web.ViewModels.Api.Categories
{
    public class CategoryImageModelValidator : AbstractValidator<CategoryImageModel>
    {
        public CategoryImageModelValidator()
        {
            RuleFor(cm => cm.CategoryID).NotEmpty();
        }
    }
}
