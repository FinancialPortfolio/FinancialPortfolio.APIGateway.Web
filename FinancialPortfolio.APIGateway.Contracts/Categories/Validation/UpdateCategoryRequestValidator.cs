using FinancialPortfolio.APIGateway.Contracts.Categories.Requests;
using FluentValidation;

namespace FinancialPortfolio.APIGateway.Contracts.Categories.Validation
{
    public class UpdateCategoryRequestValidator : AbstractValidator<UpdateCategoryRequest> 
    {
        public UpdateCategoryRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(50);
            RuleFor(x => x.ExpectedAllocation).NotNull();
            RuleFor(x => x.UserId).NotNull();
            
            RuleForEach(x => x.Stocks).SetValidator(new CategoryStockValidator());
            RuleForEach(x => x.SubCategories).SetValidator(new SubCategoryValidator());

        }
    }
}