using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FinancialPortfolio.APIGateway.Contracts.Orders.Requests;
using Google.Protobuf.Collections;
using SearchLibrary;
using OrderApi;
using AssetApi;

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
            
            CreateMap<AssetResponse, Contracts.Orders.Responses.AssetResponse>();
            
            CreateMap<(OrderResponse order, AssetResponse asset), Contracts.Orders.Responses.OrderResponse>()
                .ForMember(s => s.Id, o => o.MapFrom(d => d.order.Id))
                .ForMember(s => s.Type, o => o.MapFrom(d => d.order.Type))
                .ForMember(s => s.Amount, o => o.MapFrom(d => d.order.Amount))
                .ForMember(s => s.Price, o => o.MapFrom(d => d.order.Price))
                .ForMember(s => s.DateTime, o => o.MapFrom(d => d.order.DateTime))
                .ForMember(s => s.Commission, o => o.MapFrom(d => d.order.Commission))
                .ForMember(s => s.Asset, o => o.MapFrom(d => d.asset));
            
            CreateMap<(RepeatedField<OrderResponse> orders, RepeatedField<AssetResponse> assets), IEnumerable<Contracts.Orders.Responses.OrderResponse>>()
                .ConvertUsing(new OrderResponseConverter());
            
            CreateMap<(Guid value, string name), GetOrdersQuery>()
                .ForPath(q => q.Search.FilteringOptions, o => o.MapFrom(r =>
                    new FilteringOptions
                    {
                        Criteria =
                        {
                            new FilterCriteria
                            {
                                Field = r.name,
                                Operator = FilterOperator.Equals,
                                Value = r.value.ToString()
                            }
                        }
                    }));
        }
        
        private class OrderResponseConverter : ITypeConverter<(RepeatedField<OrderResponse> orders, RepeatedField<AssetResponse> assets), IEnumerable<Contracts.Orders.Responses.OrderResponse>>
        {
            public IEnumerable<Contracts.Orders.Responses.OrderResponse> Convert(
                (RepeatedField<OrderResponse> orders, RepeatedField<AssetResponse> assets) source, IEnumerable<Contracts.Orders.Responses.OrderResponse> destination,
                ResolutionContext context)
            {
                var result = new List<Contracts.Orders.Responses.OrderResponse>();

                foreach (var order in source.orders)
                {
                    var asset = source.assets.First(s => s.Id == order.AssetId);
                    
                    var mappedOrder = context.Mapper.Map<Contracts.Orders.Responses.OrderResponse>((order, asset));
                    result.Add(mappedOrder);   
                }

                return result;
            }
        }
    }
}