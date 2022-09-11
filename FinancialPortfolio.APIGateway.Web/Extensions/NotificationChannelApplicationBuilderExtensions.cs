using FinancialPortfolio.APIGateway.Web.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace FinancialPortfolio.APIGateway.Web.Extensions
{
    public static class NotificationChannelApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseNotificationChannelMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<NotificationChannelOperationContextMiddleware>();

            return app;
        }
    }
}