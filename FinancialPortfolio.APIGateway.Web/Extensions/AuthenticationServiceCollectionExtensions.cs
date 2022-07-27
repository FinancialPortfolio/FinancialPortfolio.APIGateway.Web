using FinancialPortfolio.APIGateway.Web.Models.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FinancialPortfolio.APIGateway.Web.Extensions
{
    public static class AuthenticationServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            var authenticationSettings = configuration.GetSection(nameof(AuthenticationSettings)).Get<AuthenticationSettings>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = authenticationSettings.Authority;
                    options.Audience = authenticationSettings.Audience;
                    options.RequireHttpsMetadata = false;

                    if (env.IsDevelopment())
                    {
                        options.TokenValidationParameters.ValidIssuers = authenticationSettings.ValidIssuers;
                    }
                });
            
            return services;
        }
    }
}