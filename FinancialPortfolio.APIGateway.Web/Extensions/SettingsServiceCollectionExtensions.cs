using FinancialPortfolio.APIGateway.Web.Models.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinancialPortfolio.APIGateway.Web.Extensions
{
    public static class SettingsServiceCollectionExtensions
    {
        public static IServiceCollection AddSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ServicesSettings>(configuration.GetSection(nameof(ServicesSettings)));

            return services;
        }
    }
}