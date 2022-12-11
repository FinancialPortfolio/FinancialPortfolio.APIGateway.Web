using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FinancialPortfolio.APIGateway.Contracts.Categories.Responses;
using Google.Protobuf.Collections;
using OrderResponse = FinancialPortfolio.APIGateway.Contracts.Categories.Responses.OrderResponse;

namespace FinancialPortfolio.APIGateway.Web.AutoMapperProfiles
{
    public class CategoriesProfile : Profile
    {
        public CategoriesProfile()
        {
            CreateMap<OrderApi.OrderResponse, OrderResponse>();
            
            CreateMap<(CategoryApi.CategoryResponse category, RepeatedField<OrderApi.OrderResponse> orders), CategoryResponse>()
                .ConvertUsing(new CategoryResponseConverter());
        }

        private class CategoryResponseConverter : ITypeConverter<(CategoryApi.CategoryResponse category, RepeatedField<OrderApi.OrderResponse> orders), CategoryResponse>
        {
            public CategoryResponse Convert(
                (CategoryApi.CategoryResponse category, RepeatedField<OrderApi.OrderResponse> orders) source, CategoryResponse destination,
                ResolutionContext context)
            {
                var result = new CategoryResponse
                {
                    Id = Guid.Parse(source.category.Id),
                    Name = source.category.Name,
                    Description = source.category.Description,
                    ExpectedAllocation = source.category.ExpectedAllocation,
                    SubCategories = source.category.SubCategories.Select(c => MapSubCategory(c, source.orders, context))
                };

                return result;
            }
        }
        
        private static SubCategoryResponse MapSubCategory(CategoryApi.SubCategoryResponse category, RepeatedField<OrderApi.OrderResponse> orders, ResolutionContext context)
        {
            var result = new SubCategoryResponse
            {
                Name = category.Name,
                Description = category.Description,
                ExpectedAllocation = category.ExpectedAllocation,
                SubCategories = category.SubCategories.Select(c => MapSubCategory(c, orders, context)),
                Stocks = category.Stocks.Select(s => MapCategoryStock(s, orders, context))
            };
            
            return result;
        }
        
        private static CategoryStockResponse MapCategoryStock(CategoryApi.CategoryStockResponse stock, RepeatedField<OrderApi.OrderResponse> orders, ResolutionContext context)
        {
            var stockOrders = orders.Where(order => order.AssetId == stock.AssetId);
            var result = new CategoryStockResponse
            {
                Name = stock.Name,
                Symbol = stock.Symbol,
                AssetId = Guid.Parse(stock.AssetId),
                ExpectedAllocation = stock.ExpectedAllocation,
                Orders = context.Mapper.Map<List<OrderResponse>>(stockOrders)
            };

            return result;
        }
    }
}