using System;
using CsvHelper.Configuration.Attributes;
using FinancialPortfolio.APIGateway.Integrations.AdmiralMarkets.Helpers;

namespace FinancialPortfolio.APIGateway.Integrations.AdmiralMarkets.Models
{
    public class TransferDeal
    {
        [Index(0)]
        public DateTime DateTime { get; set; }
        
        [Index(3)]
        public string Type { get; set; }
        
        [Index(11)]
        public string ProfitString { get; set; }
        
        public decimal Profit => ParseHelper.Parse(ProfitString);
    }
}