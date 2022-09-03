using FinancialPortfolio.Search.Pagination;
using FinancialPortfolio.Search.Sorting;

namespace FinancialPortfolio.APIGateway.Contracts.Equity.Requests
{
    public record GetTransfersRequest
    {
        public PaginationOptions Pagination { get; set; }
        
        public SortingOptions Sorting { get; set; }
    }
}