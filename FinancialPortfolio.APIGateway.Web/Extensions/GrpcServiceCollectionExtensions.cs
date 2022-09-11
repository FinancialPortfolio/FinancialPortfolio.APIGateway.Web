using System;
using AccountApi;
using FinancialPortfolio.APIGateway.Web.Models.Settings;
using FinancialPortfolio.Operations.Grpc;
using Grpc.Net.ClientFactory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderApi;
using StockApi;
using TransferApi;

namespace FinancialPortfolio.APIGateway.Web.Extensions
{
    public static class GrpcServiceCollectionExtensions
    {
        public static IServiceCollection AddGrpcClients(this IServiceCollection services, IConfiguration configuration)
        {
            var servicesSettings = configuration.GetSection(nameof(ServicesSettings)).Get<ServicesSettings>();

            services
                .AddGrpcClient<Account.AccountClient>(servicesSettings.AccountsService.GrpcUrl)
                .AddGrpcClient<Stock.StockClient>(servicesSettings.AssetsService.GrpcUrl)
                .AddGrpcClient<Transfer.TransferClient>(servicesSettings.EquityService.GrpcUrl)
                .AddGrpcClient<Order.OrderClient>(servicesSettings.OrdersService.GrpcUrl);

            return services;
        }

        private static IServiceCollection AddGrpcClient<TClient>(this IServiceCollection services, string uri) where TClient : class
        {
            services
                .AddGrpcClient<TClient>(o =>
                {
                    o.Address = new Uri(uri);
                })
                .AddInterceptor<OperationContextClientInterceptor>(InterceptorScope.Client);

            return services;
        }
    }
}