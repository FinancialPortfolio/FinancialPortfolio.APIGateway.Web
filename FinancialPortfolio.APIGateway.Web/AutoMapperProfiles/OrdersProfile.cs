using System;
using AutoMapper;
using FinancialPortfolio.APIGateway.Contracts.Orders.Requests;
using SearchLibrary;
using OrderApi;

namespace FinancialPortfolio.APIGateway.Web.AutoMapperProfiles
{
    public class OrdersProfile : Profile
    {
        public OrdersProfile()
        {
            CreateMap<(GetOrdersRequest request, Guid accountId), GetOrdersQuery>()
                .ForPath(q => q.Search.PaginationOptions, o => o.MapFrom(r => r.request.Pagination))
                .ForPath(q => q.Search.SortingOptions, o => o.MapFrom(r => r.request.Sorting))
                .ForPath(q => q.Search.FilteringOptions, o => o.MapFrom(r =>
                    new FilteringOptions
                    {
                        Criteria =
                        {
                            new FilterCriteria
                            {
                                Field = "AccountId",
                                Operator = FilterOperator.Equals,
                                Value = r.accountId.ToString()
                            }
                        }
                    }));
        }
    }
}