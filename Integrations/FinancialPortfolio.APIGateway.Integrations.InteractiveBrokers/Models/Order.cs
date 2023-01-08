using System;
using System.Globalization;
using CsvHelper.Configuration.Attributes;
using FinancialPortfolio.APIGateway.Contracts.Orders.Enums;

namespace FinancialPortfolio.APIGateway.Integrations.InteractiveBrokers.Models
{
    public class Order
    {
        [Index(4)]
        public string Currency { get; set; }
        
        [Index(5)]
        public string FullSymbol { get; set; }

        public string Symbol => FullSymbol.Split(".")[0];
        
        public string Exchange
        {
            get
            {
                var split = FullSymbol.Split(".");
                return split.Length > 1 ? split[1] : null;
            }
        }

        [Index(6)]
        public DateTime DateTime { get; set; }
        
        [Index(8)]
        public int Quantity { get; set; }
        
        [Index(9)]
        public string PriceString { get; set; }
        
        public decimal Price => decimal.Parse(PriceString, CultureInfo.InvariantCulture);

        [Index(11)]
        public string ProceedsString { get; set; }
        
        public decimal Proceeds => decimal.Parse(ProceedsString, CultureInfo.InvariantCulture);
        
        public OrderType Type => Proceeds >= 0 ? OrderType.Sell : OrderType.Buy;
        
        [Index(12)]
        public string CommissionString { get; set; }
        
        public decimal Commission => decimal.Parse(CommissionString.Replace('.', ','));
    }
}