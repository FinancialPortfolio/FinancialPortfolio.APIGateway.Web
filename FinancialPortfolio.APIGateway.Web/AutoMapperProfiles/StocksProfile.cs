using AutoMapper;
using FinancialPortfolio.APIGateway.Contracts.Assets.Requests;
using StockApi;

namespace FinancialPortfolio.APIGateway.Web.AutoMapperProfiles
{
    public class StocksProfile : Profile
    {
        public StocksProfile()
        {
            CreateMap<GetStocksRequest, GetStocksQuery>()
                .ForPath(q => q.Search.PaginationOptions, o => o.MapFrom(r => r.Pagination))
                .ForPath(q => q.Search.SortingOptions, o => o.MapFrom(r => r.Sorting));
        }
    }
}