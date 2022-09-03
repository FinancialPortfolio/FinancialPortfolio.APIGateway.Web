using System;
using AutoMapper;
using FinancialPortfolio.APIGateway.Contracts.Equity.Requests;
using SearchLibrary;
using TransferApi;

namespace FinancialPortfolio.APIGateway.Web.AutoMapperProfiles
{
    public class TransfersProfile : Profile
    {
        public TransfersProfile()
        {
            CreateMap<(GetTransfersRequest request, Guid accountId), GetTransfersQuery>()
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