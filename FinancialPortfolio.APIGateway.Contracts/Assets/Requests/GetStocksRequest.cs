using FinancialPortfolio.Search.Pagination;
using FinancialPortfolio.Search.Sorting;

namespace FinancialPortfolio.APIGateway.Contracts.Assets.Requests
{
    public record GetStocksRequest
    {
        public PaginationOptions Pagination { get; set; }
        
        public SortingOptions Sorting { get; set; }
    }
}