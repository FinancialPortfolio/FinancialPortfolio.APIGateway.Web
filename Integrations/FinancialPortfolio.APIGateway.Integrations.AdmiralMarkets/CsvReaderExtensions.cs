using System;
using CsvHelper;

namespace FinancialPortfolio.APIGateway.Integrations.AdmiralMarkets
{
    public static class CsvReaderExtensions
    {
        public static bool TryGetRecord<T>(this CsvReader reader, out T result)
        {
            try
            {
                result = reader.GetRecord<T>();
                
                return true;
            }
            catch (Exception)
            {
                result = default;
                
                return false;
            }
        }
    }
}