using FinancialPortfolio.APIGateway.Contracts.Orders.Requests;
using FluentValidation;

namespace FinancialPortfolio.APIGateway.Contracts.Orders.Validation
{
    public class UpdateOrderRequestValidator : AbstractValidator<UpdateOrderRequest>
    {
        public UpdateOrderRequestValidator()
        {
            RuleFor(x => x.Type).IsInEnum().NotNull();
            RuleFor(x => x.Amount).GreaterThan(0);
            RuleFor(x => x.Price).GreaterThan(0);
            RuleFor(x => x.Commission);
            RuleFor(x => x.DateTime).NotEmpty();
            RuleFor(x => x.AssetId).NotEmpty();
        }
    }
}