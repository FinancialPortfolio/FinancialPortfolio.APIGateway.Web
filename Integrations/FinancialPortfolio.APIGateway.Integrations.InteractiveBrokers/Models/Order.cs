using System;
using CsvHelper.Configuration.Attributes;
using FinancialPortfolio.APIGateway.Contracts.Orders.Enums;

namespace FinancialPortfolio.APIGateway.Integrations.InteractiveBrokers.Models
{
    public class Order
    {
        [Index(5)]
        public string Symbol { get; set; }
        
        [Index(6)]
        public DateTime DateTime { get; set; }
        
        [Index(8)]
        public int Quantity { get; set; }
        
        [Index(9)]
        public string PriceString { get; set; }
        
        public decimal Price => decimal.Parse(PriceString.Replace('.', ','));

        [Index(11)]
        public string ProceedsString { get; set; }
        
        public decimal Proceeds => decimal.Parse(ProceedsString.Replace('.', ','));
        
        public OrderType Type => Proceeds >= 0 ? OrderType.Sell : OrderType.Buy;
        
        [Index(12)]
        public string CommissionString { get; set; }
        
        public decimal Commission => decimal.Parse(CommissionString.Replace('.', ','));
    }
}