using System;
using System.Collections.Generic;
using System.Linq;
using FinancialPortfolio.APIGateway.Contracts.Orders.Enums;

namespace FinancialPortfolio.APIGateway.Contracts.Assets.Responses
{
    public record AccountStockResponse
    {
        public Guid Id { get; set; }
        
        public string Symbol { get; set; }
        
        public string Name { get; set; }
        
        public string Exchange { get; set; }
        
        public StockStatisticsResponse StockStatistics { get; set; }

        public double NumberOfShares => Orders.Aggregate(0.0, (totalShares, order) =>
        {
            if (order.Type == OrderType.Buy) {
                return totalShares + order.Amount;
            }

            return totalShares - order.Amount;
        });

        public List<OrderResponse> Orders { get; set; }
    }
}