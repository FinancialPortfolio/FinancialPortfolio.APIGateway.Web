using System.Net;
using FinancialPortfolio.ProblemDetails.WebApi.Models;
using Grpc.Core;

namespace FinancialPortfolio.APIGateway.Web.ProblemDetails
{
    public class RpcProblemDetails : WebApiProblemDetails
    {
        public RpcProblemDetails(RpcException exception)
        {
            Title = exception.Status.Detail;
            Detail = exception.StackTrace;
            StatusCode = GetStatusCode(exception.StatusCode);
        }

        private static HttpStatusCode GetStatusCode(StatusCode statusCode)
        {
            return statusCode switch
            { 
                Grpc.Core.StatusCode.AlreadyExists => HttpStatusCode.BadRequest,
                Grpc.Core.StatusCode.InvalidArgument => HttpStatusCode.BadRequest,
                Grpc.Core.StatusCode.OutOfRange => HttpStatusCode.BadRequest,
                Grpc.Core.StatusCode.NotFound => HttpStatusCode.NotFound,
                Grpc.Core.StatusCode.Unknown => HttpStatusCode.InternalServerError,
                _ => HttpStatusCode.InternalServerError
            };
        }
    }
}