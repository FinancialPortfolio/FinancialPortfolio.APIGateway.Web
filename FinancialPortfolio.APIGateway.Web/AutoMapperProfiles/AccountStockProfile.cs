using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FinancialPortfolio.APIGateway.Contracts.Assets.Requests;
using FinancialPortfolio.APIGateway.Contracts.Assets.Responses;
using Google.Protobuf.Collections;
using Newtonsoft.Json;
using OrderApi;
using SearchLibrary;
using StockApi;
using OrderResponse = OrderApi.OrderResponse;
using StockStatisticsResponse = StockApi.StockStatisticsResponse;

namespace FinancialPortfolio.APIGateway.Web.AutoMapperProfiles
{
    public class AccountStockProfile : Profile
    {
        public AccountStockProfile()
        {
            CreateMap<(IEnumerable<string> stockIds, GetAccountStocksRequest request), GetStocksQuery>()
                .ForPath(q => q.Search.FilteringOptions, o => o.MapFrom(r => MapFilteringOptions(r.stockIds, r.request)));
            
            CreateMap<IEnumerable<string>, GetStocksQuery>()
                .ForPath(q => q.Search.FilteringOptions, o => o.MapFrom(r => MapFilteringOptions(r, null)));
            
            CreateMap<StockResponse, GetOrdersQuery>()
                .ForPath(q => q.Search.FilteringOptions, o => o.MapFrom(r =>
                    new FilteringOptions
                    {
                        Criteria =
                        {
                            new FilterCriteria
                            {
                                Field = "AssetId",
                                Operator = FilterOperator.Equals,
                                Value = r.Id
                            }
                        }
                    }));
            
            CreateMap<Guid, GetOrdersQuery>()
                .ForPath(q => q.Search.FilteringOptions, o => o.MapFrom(r =>
                    new FilteringOptions
                    {
                        Criteria =
                        {
                            new FilterCriteria
                            {
                                Field = "AccountId",
                                Operator = FilterOperator.Equals,
                                Value = r.ToString()
                            }
                        }
                    }));
            
            CreateMap<OrderResponse, Contracts.Assets.Responses.OrderResponse>();

            CreateMap<StockStatisticsResponse, Contracts.Assets.Responses.StockStatisticsResponse>();
            
            CreateMap<(StockResponse stock, IEnumerable<OrderResponse> orders), AccountStockResponse>()
                .ForMember(s => s.Id, o => o.MapFrom(d => d.stock.Id))
                .ForMember(s => s.Name, o => o.MapFrom(d => d.stock.Name))
                .ForMember(s => s.Symbol, o => o.MapFrom(d => d.stock.Symbol))
                .ForMember(s => s.Exchange, o => o.MapFrom(d => d.stock.Exchange))
                .ForMember(s => s.StockStatistics, o => o.MapFrom(d => d.stock.StockStatistics))
                .ForMember(s => s.Orders, o => o.MapFrom(d => d.orders));

            CreateMap<(RepeatedField<StockResponse> stocks, RepeatedField<OrderResponse> orders), IEnumerable<AccountStockResponse>>()
                .ConvertUsing(new AccountStockResponseConverter());
        }
        
        private static FilteringOptions MapFilteringOptions(IEnumerable<string> stockIds, GetAccountStocksRequest request = null)
        {
            var filteringOptions = new FilteringOptions
            {
                Criteria =
                {
                    new FilterCriteria
                    {
                        Field = "Id",
                        Operator = FilterOperator.In,
                        Value = JsonConvert.SerializeObject(stockIds)
                    }
                }
            };

            if (!string.IsNullOrEmpty(request?.Type))
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
        
        private class AccountStockResponseConverter : ITypeConverter<(RepeatedField<StockResponse> stocks, RepeatedField<OrderResponse> orders), IEnumerable<AccountStockResponse>>
        {
            public IEnumerable<AccountStockResponse> Convert(
                (RepeatedField<StockResponse> stocks, RepeatedField<OrderResponse> orders) source, IEnumerable<AccountStockResponse> destination,
                ResolutionContext context)
            {
                var result = new List<AccountStockResponse>();

                foreach (var stock in source.stocks)
                {
                    var orders = source.orders.Where(order => order.AssetId == stock.Id);
                    
                    var accountStock = context.Mapper.Map<AccountStockResponse>((stock, orders));
                    result.Add(accountStock);
                }

                return result;
            }
        }
    }
}