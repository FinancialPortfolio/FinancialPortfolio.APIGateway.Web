using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FinancialPortfolio.APIGateway.Web.Extensions;
using FinancialPortfolio.Infrastructure.Extensions;

namespace FinancialPortfolio.APIGateway.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        private readonly string EnvironmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            
            services
                .AddSwagger()
                .AddKafkaCQRSMessaging(Configuration, EnvironmentName)
                .AddSettings(Configuration);
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
