using System.Net;
using FinancialPortfolio.APIGateway.Web.Models.Exceptions;
using FinancialPortfolio.APIGateway.Web.ProblemDetails;
using FinancialPortfolio.Infrastructure.WebApi.Exceptions;
using FinancialPortfolio.Infrastructure.WebApi.Extensions;
using FinancialPortfolio.ProblemDetails.WebApi.ProblemDetails;
using Grpc.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace FinancialPortfolio.APIGateway.Web.Extensions
{
    public static class ProblemDetailsServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomProblemDetails(this IServiceCollection services, IWebHostEnvironment env)
        {
            services.AddWebApiProblemDetails(env, settings =>
            {
                settings.MapStatusCode = context =>
                {
                    return (HttpStatusCode) context.Response.StatusCode switch
                    {
                        HttpStatusCode.Unauthorized => new UnauthorizedProblemDetails(),
                        HttpStatusCode.Forbidden => new ForbiddenProblemDetails(),
                        _ => null
                    };
                };

                settings.Map<RpcException>(exception => new RpcProblemDetails(exception));
                settings.Map<InvalidModelStateException>(exception => new InvalidModelStateProblemDetails(exception));
                settings.Map<ForbiddenException>(exception => new ForbiddenProblemDetails(exception));
            });
            
            return services;
        }
    }
}