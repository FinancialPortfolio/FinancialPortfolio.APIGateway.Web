using System.Threading.Tasks;
using FinancialPortfolio.APIGateway.Web.Interfaces;
using FinancialPortfolio.APIGateway.Web.Models.Constants;
using FinancialPortfolio.Operations;
using Microsoft.AspNetCore.Http;

namespace FinancialPortfolio.APIGateway.Web.Middlewares
{
    public class NotificationChannelOperationContextMiddleware
    {
        private readonly RequestDelegate _next;

        public NotificationChannelOperationContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, IUserInfoService userInfoService, OperationContext operationContext)
        {
            var claim = await userInfoService.GetClaimAsync<string>(ClaimConstants.UserId);
            if (!string.IsNullOrEmpty(claim))
                operationContext.AddNotificationChannel(claim);

            await _next(httpContext);
        }
    }
}