using FinancialPortfolio.APIGateway.Contracts.Accounts.Validation;
using FinancialPortfolio.APIGateway.Web.AutoMapperProfiles;
using FinancialPortfolio.APIGateway.Web.Extensions;
using FinancialPortfolio.APIGateway.Web.Factories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FinancialPortfolio.APIGateway.Web.Models.Settings;
using FinancialPortfolio.APIGateway.Web.Services;
using FinancialPortfolio.Infrastructure.Extensions;
using FinancialPortfolio.Infrastructure.Shared.Extensions;
using FinancialPortfolio.Infrastructure.WebApi.Extensions;
using FinancialPortfolio.Operations.Grpc.Extensions;
using FinancialPortfolio.Operations.WebApi.Extensions;
using FinancialPortfolio.ProblemDetails.WebApi.Extensions;

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
                .AddCustomProblemDetails(WebHostEnvironment)
                .AddFluentValidation(typeof(CreateAccountRequestValidator).Assembly)
                .AddCustomControllers()
                .AddCustomApiBehavior()
                .AddCustomAutoMapper(typeof(SearchProfile).Assembly)
                .AddCustomCors(Configuration)
                .AddDefaultServiceImplementations(typeof(UserInfoService).Assembly)
                .AddDefaultImplementations(typeof(IIntegrationServiceFactory).Assembly, "Factory")
                .AddMongo(Configuration)
                .AddCustomSwagger(Configuration)
                .AddKafkaCQRSMessaging(Configuration, WebHostEnvironment.EnvironmentName)
                .AddGrpcOperationContext()
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

            app.UseProblemDetails();

            app.UseHttpsRedirection();

            app.UseCustomCors();
            
            app.UseCustomSwagger();

            app.UseRouting();

            app.UseOperationContextMiddleware();

            app.UseLogging();

            app.UseAuthentication();

            app.UseAuthorization();
            
            app.UseNotificationChannelMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
