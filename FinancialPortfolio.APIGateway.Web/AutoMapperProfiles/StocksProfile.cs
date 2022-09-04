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
                .ForPath(q => q.Search.FilteringOptions, o => o.MapFrom(r =>
                    new FilteringOptions
                    {
                        Criteria =
                        {
                            new FilterCriteria
                            {
                                Field = "Name",
                                Operator = FilterOperator.Contains,
                                Value = r.Name ?? ""
                            },
                            new FilterCriteria
                            {
                                Field = "Symbol",
                                Operator = FilterOperator.Contains,
                                Value = r.Symbol ?? ""
                            }
                        }
                    }));
        }
    }
}