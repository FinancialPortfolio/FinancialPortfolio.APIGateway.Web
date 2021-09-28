using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FinancialPortfolio.APIGateway.Web.Extensions;
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
            services.AddControllers();
            
            services
                .AddDefaultServiceImplementations()
                .AddCustomSwagger(Configuration)
                .AddKafkaCQRSMessaging(Configuration, EnvironmentName)
                .AddDefaultSettings(Configuration, new [] { Assembly.GetEntryAssembly() })
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
