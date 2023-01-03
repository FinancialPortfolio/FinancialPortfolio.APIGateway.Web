using FinancialPortfolio.APIGateway.Contracts.Dividends.Requests;
using FluentValidation;

namespace FinancialPortfolio.APIGateway.Contracts.Dividends.Validation
{
    public class GetAccountDividendsRequestValidation : AbstractValidator<GetAccountDividendsRequest>
    {
        public GetAccountDividendsRequestValidation()
        {
            RuleFor(x => x.AccountIds).NotEmpty();
            RuleFor(x => x.StartDateTime).NotEmpty();
            RuleFor(x => x.EndDateTime).NotEmpty();
        }
    }
}