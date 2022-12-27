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
using AssetApi;
using OrderResponse = OrderApi.OrderResponse;
using AssetStatisticsResponse = AssetApi.AssetStatisticsResponse;

namespace FinancialPortfolio.APIGateway.Web.AutoMapperProfiles
{
    public class AccountAssetProfile : Profile
    {
        public AccountAssetProfile()
        {
            CreateMap<(IEnumerable<Guid> assetIds, GetAccountAssetsRequest request), GetAssetsQuery>()
                .ForPath(q => q.Search.FilteringOptions, o => o.MapFrom(r => MapFilteringOptions(r.assetIds)))
                .ForPath(q => q.Type, o => o.MapFrom(r => r.request.Type ?? ""));;
            
            CreateMap<IEnumerable<Guid>, GetAssetsQuery>()
                .ForPath(q => q.Search.FilteringOptions, o => o.MapFrom(r => MapFilteringOptions(r)));
            
            CreateMap<AssetResponse, GetOrdersQuery>()
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

            CreateMap<OrderResponse, Contracts.Assets.Responses.OrderResponse>();

            CreateMap<AssetStatisticsResponse, Contracts.Assets.Responses.AssetStatisticsResponse>();
            
            CreateMap<(AssetResponse asset, IEnumerable<OrderResponse> orders), AccountAssetResponse>()
                .ForMember(s => s.Id, o => o.MapFrom(d => d.asset.Id))
                .ForMember(s => s.Name, o => o.MapFrom(d => d.asset.Name))
                .ForMember(s => s.Symbol, o => o.MapFrom(d => d.asset.Symbol))
                .ForMember(s => s.Exchange, o => o.MapFrom(d => d.asset.Exchange))
                .ForMember(s => s.AssetStatistics, o => o.MapFrom(d => d.asset.AssetStatistics))
                .ForMember(s => s.Orders, o => o.MapFrom(d => d.orders));

            CreateMap<(RepeatedField<AssetResponse> assets, RepeatedField<OrderResponse> orders), IEnumerable<AccountAssetResponse>>()
                .ConvertUsing(new AccountAssetResponseConverter());
        }
        
        private static FilteringOptions MapFilteringOptions(IEnumerable<Guid> assetIds)
        {
            var filteringOptions = new FilteringOptions
            {
                Criteria =
                {
                    new FilterCriteria
                    {
                        Field = "Id",
                        Operator = FilterOperator.In,
                        Value = JsonConvert.SerializeObject(assetIds)
                    }
                }
            };

            return filteringOptions;
        }
        
        private class AccountAssetResponseConverter : ITypeConverter<(RepeatedField<AssetResponse> assets, RepeatedField<OrderResponse> orders), IEnumerable<AccountAssetResponse>>
        {
            public IEnumerable<AccountAssetResponse> Convert(
                (RepeatedField<AssetResponse> assets, RepeatedField<OrderResponse> orders) source, IEnumerable<AccountAssetResponse> destination,
                ResolutionContext context)
            {
                var result = new List<AccountAssetResponse>();

                foreach (var asset in source.assets)
                {
                    var orders = source.orders.Where(order => order.AssetId == asset.Id);
                    
                    var accountAsset = context.Mapper.Map<AccountAssetResponse>((asset, orders));
                    result.Add(accountAsset);
                }

                return result;
            }
        }
    }
}