using FinancialPortfolio.APIGateway.Contracts.Categories.Models;
using FluentValidation;

namespace FinancialPortfolio.APIGateway.Contracts.Categories.Validation
{
    public class CategoryStockValidator : AbstractValidator<CategoryStock> 
    {
        public CategoryStockValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Symbol).NotEmpty().MaximumLength(50);
            RuleFor(x => x.AssetId).NotNull();
        }
    }
}