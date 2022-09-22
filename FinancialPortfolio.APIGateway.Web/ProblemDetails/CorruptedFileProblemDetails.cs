using System;
using System.Net;
using FinancialPortfolio.ProblemDetails.WebApi.Models;

namespace FinancialPortfolio.APIGateway.Web.ProblemDetails
{
    public class CorruptedFileProblemDetails : WebApiProblemDetails
    {
        public CorruptedFileProblemDetails(Exception exception)
        {
            Title = "File contains corrupted data.";
            Detail = exception.StackTrace;
            StatusCode = HttpStatusCode.BadRequest;
        }
    }
}