using System;

namespace FinancialPortfolio.APIGateway.Contracts.Assets.Responses
{
    public record StockStatisticsResponse
    {
        public decimal CurrentPrice { get; set; }
        
        public DateTime RetrievalDateTime { get; set; }
        
        public double EarningsPerShare { get; set; }
        
        public double Beta { get; set; }
        
        public double MarketCapitalization { get; set; }
        
        public double DividendYield { get; set; }
        
        public double PriceToBookValue { get; set; }
        
        public double PriceToEarningsValue { get; set; }
        
        public double PriceToSalesValue { get; set; }
        
        public string Logo { get; set; }
    }
}