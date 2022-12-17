using FinancialPortfolio.APIGateway.Contracts.Categories.Models;
using FluentValidation;

namespace FinancialPortfolio.APIGateway.Contracts.Categories.Validation
{
    public class CategoryAssetValidator : AbstractValidator<CategoryAsset> 
    {
        public CategoryAssetValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Symbol).NotEmpty().MaximumLength(50);
            RuleFor(x => x.AssetId).NotNull();
        }
    }
}