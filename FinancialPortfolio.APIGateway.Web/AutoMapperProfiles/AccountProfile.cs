using System;
using AccountApi;
using AutoMapper;
using SearchLibrary;

namespace FinancialPortfolio.APIGateway.Web.AutoMapperProfiles
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<Guid, GetAccountsQuery>()
                .ForPath(q => q.Search.FilteringOptions, o => o.MapFrom(userId =>
                    new FilteringOptions
                    {
                        Criteria =
                        {
                            new FilterCriteria
                            {
                                Field = "UserId",
                                Operator = FilterOperator.Equals,
                                Value = userId.ToString()
                            }
                        }
                    }));
        }
    }
}