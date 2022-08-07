using AutoMapper;
using FinancialPortfolio.Search;
using FinancialPortfolio.Search.Filtering;
using FinancialPortfolio.Search.Pagination;
using FinancialPortfolio.Search.Sorting;

namespace FinancialPortfolio.APIGateway.Web.AutoMapperProfiles
{
    public class SearchProfile : Profile
    {
        public SearchProfile()
        {
            CreateMap<SearchOptions, SearchLibrary.SearchOptions>();
            
            CreateMap<FilteringOptions, SearchLibrary.FilteringOptions>();
            CreateMap<FilterCriteria, SearchLibrary.FilterCriteria>();
            
            CreateMap<SortingOptions, SearchLibrary.SortingOptions>();
            CreateMap<PaginationOptions, SearchLibrary.PaginationOptions>();
        }
    }
}