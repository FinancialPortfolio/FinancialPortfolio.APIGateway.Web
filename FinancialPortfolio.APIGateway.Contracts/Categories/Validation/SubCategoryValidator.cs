using FinancialPortfolio.APIGateway.Contracts.Categories.Models;
using FluentValidation;

namespace FinancialPortfolio.APIGateway.Contracts.Categories.Validation
{
    public class SubCategoryValidator : AbstractValidator<SubCategory> 
    {
        public SubCategoryValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(50);
            RuleFor(x => x.ExpectedAllocation).NotNull();
            
            RuleForEach(x => x.Stocks).SetValidator(new CategoryStockValidator());
            RuleForEach(x => x.SubCategories).SetValidator(new SubCategoryValidator());
        }
    }
}