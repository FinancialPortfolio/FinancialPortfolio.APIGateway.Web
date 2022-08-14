using FinancialPortfolio.APIGateway.Contracts.Assets.Requests;
using FluentValidation;

namespace FinancialPortfolio.APIGateway.Contracts.Assets.Validation
{ 
    public class CreateAssetRequestValidator  : AbstractValidator<CreateAssetRequest>
    {
        public CreateAssetRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Symbol).NotEmpty().MaximumLength(10);
            RuleFor(x => x.Type).IsInEnum().NotNull();
        }
    }
}