using System.Collections.Generic;
using AutoMapper;
using Newtonsoft.Json;
using SearchLibrary;
using StockApi;

namespace FinancialPortfolio.APIGateway.Web.AutoMapperProfiles
{
    public class IntegrationProfile : Profile
    {
        public IntegrationProfile()
        {
            CreateMap<IEnumerable<string>, GetStocksQuery>()
                .ForPath(q => q.Search.FilteringOptions, o => o.MapFrom(symbols =>
                    new FilteringOptions
                    {
                        Criteria =
                        {
                            new FilterCriteria
                            {
                                Field = "Symbol",
                                Operator = FilterOperator.In,
                                Value = JsonConvert.SerializeObject(symbols)
                            }
                        }
                    }));
        }
    }
}