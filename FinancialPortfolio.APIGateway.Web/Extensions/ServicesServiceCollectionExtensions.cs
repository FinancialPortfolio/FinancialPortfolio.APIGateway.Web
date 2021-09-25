using FinancialPortfolio.APIGateway.Web.Services;
using FinancialPortfolio.APIGateway.Web.Services.Abstraction;
using Microsoft.Extensions.DependencyInjection;

namespace FinancialPortfolio.APIGateway.Web.Extensions
{
    public static class ServicesServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddTransient<IUserInfoService, UserInfoService>();

            return services;
        }
    }
}