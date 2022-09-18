using System;
using CsvHelper.Configuration.Attributes;

namespace FinancialPortfolio.APIGateway.Integrations.AdmiralMarkets.Models
{
    public class Deal
    {
        [Index(0)]
        public DateTime DateTime { get; set; }
        
        [Index(2)]
        public string Symbol { get; set; }
        
        [Index(3)]
        public string Type { get; set; }
        
        [Index(4)]
        public string Direction { get; set; }
        
        [Index(5)]
        public string VolumeString { get; set; }
        
        public int Volume => int.Parse(VolumeString);
        
        [Index(6)]
        public string PriceString { get; set; }

        public decimal Price => decimal.Parse(PriceString);
        
        [Index(7)]
        public string OrderId { get; set; }

        [Index(8)]
        public string CommissionString { get; set; }
        
        public decimal Commission => decimal.Parse(CommissionString.Replace('.', ','));
        
        [Index(9)]
        public string FeeString { get; set; }
        
        public decimal Fee => decimal.Parse(FeeString.Replace('.', ','));
    }
}