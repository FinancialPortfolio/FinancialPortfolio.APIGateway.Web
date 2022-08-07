using FinancialPortfolio.APIGateway.Web.AutoMapperProfiles;
using FinancialPortfolio.APIGateway.Web.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FinancialPortfolio.APIGateway.Web.Models.Settings;
using FinancialPortfolio.APIGateway.Web.Services;
using FinancialPortfolio.Infrastructure.Extensions;
using FinancialPortfolio.Infrastructure.WebApi.Extensions;
using FinancialPortfolio.Logging.Grpc.Extensions;
using FinancialPortfolio.Logging.Messaging.Extensions;

namespace FinancialPortfolio.APIGateway.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            WebHostEnvironment = webHostEnvironment;
        }

        private IConfiguration Configuration { get; }
        
        private IWebHostEnvironment WebHostEnvironment { get; } 
        
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddCustomControllers()
                .AddCustomAutoMapper(typeof(SearchProfile).Assembly)
                .AddCustomCors()
                .AddDefaultServiceImplementations(typeof(UserInfoService).Assembly)
                .AddCustomSwagger(Configuration)
                .AddKafkaCQRSMessaging(Configuration, WebHostEnvironment.EnvironmentName)
                .AddMessagingLogging(Configuration)
                .AddGrpcLogging()
                .AddDefaultSettings(Configuration, typeof(ServicesSettings).Assembly)
                .AddCustomAuthentication(Configuration, WebHostEnvironment)
                .AddCustomAuthorization()
                .AddHttpContextAccessor()
                .AddGrpcClients(Configuration);
        }

        public void Configure(IApplicationBuilder app)
        {
            if (WebHostEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseCustomCors();
            
            app.UseCustomSwagger();

            app.UseRouting();

            app.UseLogging();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
