using FinancialPortfolio.APIGateway.Web.Models.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinancialPortfolio.APIGateway.Web.Extensions
{
    public static class AuthenticationServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var authenticationSettings = configuration.GetSection(nameof(AuthenticationSettings)).Get<AuthenticationSettings>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = authenticationSettings.Authority;
                    options.Audience = authenticationSettings.Audience;
                    options.RequireHttpsMetadata = false;
                });
            
            return services;
        }
    }
}