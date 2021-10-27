using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FinancialPortfolio.APIGateway.Web.Extensions;
using FinancialPortfolio.APIGateway.Web.Models.Settings;
using FinancialPortfolio.APIGateway.Web.Services;
using FinancialPortfolio.Infrastructure.Extensions;
using FinancialPortfolio.Infrastructure.WebApi.Extensions;

namespace FinancialPortfolio.APIGateway.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }
        
        private readonly string EnvironmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddCustomControllers()
                .AddCustomCors()
                .AddDefaultServiceImplementations(typeof(UserInfoService).Assembly)
                .AddCustomSwagger(Configuration)
                .AddKafkaCQRSMessaging(Configuration, EnvironmentName)
                .AddDefaultSettings(Configuration, typeof(ServicesSettings).Assembly)
                .AddCustomAuthentication(Configuration)
                .AddCustomAuthorization()
                .AddHttpContextAccessor();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseCustomCors();
            
            app.UseCustomSwagger();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
