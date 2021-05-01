using System;
using System.IO;
using FinancialPortfolio.APIGateway.Web.Settings;
using FinancialPortfolio.Messaging.Kafka;
using FinancialPortfolio.Messaging.Kafka.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace FinancialPortfolio.APIGateway.Web.Extensions
{
    public static class InfrastructureExtensions
    {
        public static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration)
        {
            var kafkaSettings = configuration.GetSection(nameof(MessagingSettings)).Get<MessagingSettings>();

            var messagingOptions = new KafkaMessagingOptions()
            {
                Prefix = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
                BootstrapServers = kafkaSettings.BootstrapServers,
                GroupId = kafkaSettings.GroupId,
                TimeoutTime = TimeSpan.FromSeconds(10)
            };
            
            services.AddKafkaMessaging(messagingOptions);
            
            return services;
        }
        
        public static IServiceCollection AddSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ServicesSettings>(configuration.GetSection(nameof(ServicesSettings)));

            return services;
        }
        
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Gateway", Version = "v1" });

                var apiGatewayXmlPath = Path.Combine(AppContext.BaseDirectory, "Gateway.xml");
                var contractsXmlPath = Path.Combine(AppContext.BaseDirectory, "Gateway.Contracts.xml");

                if (File.Exists(apiGatewayXmlPath))
                {
                    c.IncludeXmlComments(apiGatewayXmlPath, true);
                }

                if (File.Exists(contractsXmlPath))
                {
                    c.IncludeXmlComments(contractsXmlPath);
                }
            });

            return services;
        }
        
        public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Gateway");
                c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
                c.RoutePrefix = "swagger";
            });

            return app;
        }
    }
}