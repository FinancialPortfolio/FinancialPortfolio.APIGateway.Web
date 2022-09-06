using FinancialPortfolio.Search.Pagination;
using FinancialPortfolio.Search.Sorting;

namespace FinancialPortfolio.APIGateway.Contracts.Orders.Requests
{
    public record GetOrdersRequest
    {
        public PaginationOptions Pagination { get; set; }
        
        public SortingOptions Sorting { get; set; }
    }
}