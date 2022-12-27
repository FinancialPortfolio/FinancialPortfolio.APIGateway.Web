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
                    ExpectedAllocationInPercentage = source.category.ExpectedAllocationInPercentage,
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
                ExpectedAllocationInPercentage = category.ExpectedAllocationInPercentage,
                SubCategories = category.SubCategories.Select(c => MapSubCategory(c, orders, context)),
                Assets = category.Assets.Select(s => MapCategoryAsset(s, orders, context))
            };
            
            return result;
        }
        
        private static CategoryAssetResponse MapCategoryAsset(CategoryApi.CategoryAssetResponse asset, RepeatedField<OrderApi.OrderResponse> orders, ResolutionContext context)
        {
            var assetOrders = orders.Where(order => order.AssetId == asset.AssetId);
            var result = new CategoryAssetResponse
            {
                Name = asset.Name,
                Symbol = asset.Symbol,
                Type = asset.Type,
                AssetId = Guid.Parse(asset.AssetId),
                ExpectedAllocationInPercentage = asset.ExpectedAllocationInPercentage,
                Orders = context.Mapper.Map<List<OrderResponse>>(assetOrders)
            };

            return result;
        }
    }
}