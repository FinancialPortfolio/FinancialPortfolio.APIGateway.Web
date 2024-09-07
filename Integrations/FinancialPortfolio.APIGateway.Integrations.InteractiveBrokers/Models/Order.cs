using System;
using CsvHelper.Configuration.Attributes;
using FinancialPortfolio.APIGateway.Contracts.Orders.Enums;
using FinancialPortfolio.APIGateway.Integrations.InteractiveBrokers.Helpers;

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
        
        public decimal Price => ParseHelper.Parse(PriceString);

        [Index(11)]
        public string ProceedsString { get; set; }
        
        public decimal Proceeds => ParseHelper.Parse(ProceedsString);
        
        public OrderType Type => Proceeds >= 0 ? OrderType.Sell : OrderType.Buy;
        
        [Index(12)]
        public string CommissionString { get; set; }
        
        public decimal Commission => ParseHelper.Parse(CommissionString);
    }
}