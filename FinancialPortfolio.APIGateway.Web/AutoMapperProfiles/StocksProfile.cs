using AutoMapper;
using FinancialPortfolio.APIGateway.Contracts.Assets.Requests;
using SearchLibrary;
using StockApi;

namespace FinancialPortfolio.APIGateway.Web.AutoMapperProfiles
{
    public class StocksProfile : Profile
    {
        public StocksProfile()
        {
            CreateMap<GetStocksRequest, GetStocksQuery>()
                .ForPath(q => q.Search.PaginationOptions, o => o.MapFrom(r => r.Pagination))
                .ForPath(q => q.Search.SortingOptions, o => o.MapFrom(r => r.Sorting))
                .ForPath(q => q.Search.FilteringOptions, o => o.MapFrom(r => MapFilteringOptions(r)));
        }

        private static FilteringOptions MapFilteringOptions(GetStocksRequest request)
        {
            var filteringOptions = new FilteringOptions { Criteria = {} };
            
            if (!string.IsNullOrEmpty(request.Name))
            {
                filteringOptions.Criteria.Add(new FilterCriteria
                {
                    Field = "Name",
                    Operator = FilterOperator.Contains,
                    Value = request.Name
                });
            }
            
            if (!string.IsNullOrEmpty(request.Symbol))
            {
                filteringOptions.Criteria.Add(new FilterCriteria
                {
                    Field = "Symbol",
                    Operator = FilterOperator.Contains,
                    Value = request.Symbol
                });
            }
            
            if (!string.IsNullOrEmpty(request.Type))
            {
                filteringOptions.Criteria.Add(new FilterCriteria
                {
                    Field = "Type",
                    Operator = FilterOperator.Equals,
                    Value = request.Type
                });
            }

            return filteringOptions;
        }
    }
}