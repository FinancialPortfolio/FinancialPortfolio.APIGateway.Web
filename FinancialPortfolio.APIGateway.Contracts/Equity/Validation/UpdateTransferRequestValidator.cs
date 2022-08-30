using FinancialPortfolio.APIGateway.Contracts.Equity.Requests;
using FluentValidation;

namespace FinancialPortfolio.APIGateway.Contracts.Equity.Validation
{
    public class UpdateTransferRequestValidator : AbstractValidator<UpdateTransferRequest>
    {
        public UpdateTransferRequestValidator()
        {
            RuleFor(x => x.Amount).GreaterThan(0);
            RuleFor(x => x.Type).IsInEnum().NotNull();
            RuleFor(x => x.DateTime).NotEmpty();
        }
    }
}