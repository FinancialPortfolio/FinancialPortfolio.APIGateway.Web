using FinancialPortfolio.APIGateway.Contracts.Integrations.Requests;
using FluentValidation;

namespace FinancialPortfolio.APIGateway.Contracts.Integrations.Validation
{
    public class IntegrateRequestValidator : AbstractValidator<IntegrateRequest> 
    {
        public IntegrateRequestValidator()
        {
            RuleFor(x => x.Source).IsInEnum().NotNull();
            RuleFor(x => x.File).NotNull();
        }
    }
}