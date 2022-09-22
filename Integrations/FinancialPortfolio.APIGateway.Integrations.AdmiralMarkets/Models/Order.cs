using System;
using System.Collections.Generic;
using CsvHelper.Configuration.Attributes;

namespace FinancialPortfolio.APIGateway.Integrations.AdmiralMarkets.Models
{
    public class Order
    {
        [Index(1)]
        public string OrderId { get; set; }
        
        [Index(2)]
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
        
        [Index(3)]
        public string Type { get; set; }
        
        [Index(4)]
        public string Volume { get; set; }

        [Index(8)]
        public DateTime DateTime { get; set; }
        
        [Index(9)]
        public string State { get; set; }
        
        public List<Deal> Deals { get; set; }
    }
}