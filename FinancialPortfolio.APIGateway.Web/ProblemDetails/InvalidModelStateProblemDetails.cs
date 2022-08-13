using System.Linq;
using System.Net;
using FinancialPortfolio.Infrastructure.WebApi.Exceptions;
using FinancialPortfolio.ProblemDetails.WebApi.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FinancialPortfolio.APIGateway.Web.ProblemDetails
{
    public class InvalidModelStateProblemDetails : WebApiProblemDetails
    {
        public InvalidModelStateProblemDetails(InvalidModelStateException exception)
        {
            Title = exception.Message;
            Detail = exception.StackTrace;
            StatusCode = HttpStatusCode.BadRequest;
            
            var errors = exception.ModelState
                .Where(stateItem => stateItem.Value.ValidationState != ModelValidationState.Valid);
            foreach (var error in errors)
            {
                Errors.Add(error.Key, error.Value.Errors.Select(e => e.ErrorMessage).ToArray());
            }
        }
    }
}