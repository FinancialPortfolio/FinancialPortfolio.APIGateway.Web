using FinancialPortfolio.APIGateway.Contracts.Accounts.Requests;
using FluentValidation;

namespace FinancialPortfolio.APIGateway.Contracts.Accounts.Validation
{
    public class UpdateAccountRequestValidator : AbstractValidator<UpdateAccountRequest> 
    {
        public UpdateAccountRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(50);
        }
    }
}