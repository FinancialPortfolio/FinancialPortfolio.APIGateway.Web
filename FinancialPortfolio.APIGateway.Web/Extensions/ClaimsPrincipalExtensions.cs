using System;
using System.Linq;
using System.Security.Claims;
using Newtonsoft.Json;

namespace FinancialPortfolio.APIGateway.Web.Extensions
{
    internal static class ClaimsPrincipalExtensions
    {
        public static T GetClaim<T>(this ClaimsPrincipal user, string type) where T : class
        {
            var claim = user.Claims.SingleOrDefault(c => c.Type.Equals(type, StringComparison.InvariantCulture));
            if (claim == null)
                return null;
            
            if (typeof(T) == typeof(string))
            {
                return (T) Convert.ChangeType(claim.Value, typeof(T));
            }

            return JsonConvert.DeserializeObject<T>(claim.Value);
        }
    }
}