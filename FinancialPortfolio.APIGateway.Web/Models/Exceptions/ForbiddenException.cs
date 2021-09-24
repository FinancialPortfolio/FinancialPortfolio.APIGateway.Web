using System;

namespace FinancialPortfolio.APIGateway.Web.Models.Exceptions
{
    public class ForbiddenException : Exception
    {
        public ForbiddenException(string message, params object[] parameters)
            : base(string.Format(message, parameters))
        {
        }
    }
}