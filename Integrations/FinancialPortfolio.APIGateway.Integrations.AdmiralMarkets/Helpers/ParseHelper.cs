using System.Globalization;

namespace FinancialPortfolio.APIGateway.Integrations.AdmiralMarkets.Helpers
{
    public static class ParseHelper
    {
        public static decimal Parse(string value)
        {
            return decimal.Parse(value.Replace(',', '.'), CultureInfo.InvariantCulture);
        }
    }
}