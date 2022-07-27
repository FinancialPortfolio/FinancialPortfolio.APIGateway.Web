using System.Collections.Generic;

namespace FinancialPortfolio.APIGateway.Web.Models.Settings
{
    public class AuthenticationSettings
    {
        public string Authority { get; set; }
        
        public string Audience { get; set; }
        
        public List<string> ValidIssuers { get; set; }
    }
}