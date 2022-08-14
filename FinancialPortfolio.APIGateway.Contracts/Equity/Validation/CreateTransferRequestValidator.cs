using FinancialPortfolio.APIGateway.Contracts.Equity.Requests;
using FluentValidation;

namespace FinancialPortfolio.APIGateway.Contracts.Equity.Validation
{
    public class CreateTransferRequestValidator : AbstractValidator<CreateTransferRequest>
    {
        public CreateTransferRequestValidator()
        {
            RuleFor(x => x.Amount).GreaterThan(0);
            RuleFor(x => x.Type).IsInEnum().NotNull();
            RuleFor(x => x.AccountId).NotEmpty();
        }
    }
}