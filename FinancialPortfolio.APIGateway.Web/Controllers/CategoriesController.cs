using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AssetApi;
using AutoMapper;
using CategoryApi;
using FinancialPortfolio.APIGateway.Contracts.Categories.Commands;
using FinancialPortfolio.APIGateway.Contracts.Categories.Requests;
using FinancialPortfolio.APIGateway.Web.Interfaces;
using FinancialPortfolio.CQRS.Commands;
using FinancialPortfolio.Infrastructure.WebApi.Models.Response;
using FinancialPortfolio.ProblemDetails.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderApi;
using CategoryResponse = FinancialPortfolio.APIGateway.Contracts.Categories.Responses.CategoryResponse;
using SubCategoryResponse = FinancialPortfolio.APIGateway.Contracts.Categories.Responses.SubCategoryResponse;
using CategoryAssetResponse = FinancialPortfolio.APIGateway.Contracts.Categories.Responses.CategoryAssetResponse;
using OrderResponse = FinancialPortfolio.APIGateway.Contracts.Categories.Responses.OrderResponse;

namespace FinancialPortfolio.APIGateway.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/categories")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status500InternalServerError)]
    public class CategoriesController : ApiControllerBase
    {
        private readonly ICommandPublisher _commandPublisher;
        private readonly Category.CategoryClient _categoryClient;
        private readonly Order.OrderClient _orderClient;
        private readonly Asset.AssetClient _assetClient;
        private readonly IMapper _mapper;

        public CategoriesController(
            ICommandPublisher commandPublisher, IUserInfoService userInfoService, Category.CategoryClient categoryClient, 
            Order.OrderClient orderClient, IMapper mapper, Asset.AssetClient assetClient) : base(userInfoService)
        {
            _commandPublisher = commandPublisher;
            _categoryClient = categoryClient;
            _orderClient = orderClient;
            _mapper = mapper;
            _assetClient = assetClient;
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(WebApiResponse<CategoryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WebApiResponse<CategoryResponse>>> GetAsync()
        {
            var userId = await GetUserIdAsync();
            
            var categoryQuery = new GetCategoryQuery { UserId = userId.ToString() };
            var categoryResponse = await _categoryClient.GetAsync(categoryQuery);
            
            var ordersQuery = _mapper.Map<GetOrdersQuery>((userId, "UserId"));
            var ordersResponse = await _orderClient.GetAllAsync(ordersQuery);
            
            var result = _mapper.Map<CategoryResponse>((categoryResponse, ordersResponse.Orders));

            await AddUncategorizedAssetsAsync(result, ordersResponse);
            
            return WebApiResponse.Success(result);
        }
        
        [HttpPut]
        [ProducesResponseType(typeof(WebApiResponse), StatusCodes.Status202Accepted)]
        public async Task<ActionResult<WebApiResponse>> UpdateAsync([FromBody] UpdateCategoryRequest request)
        {
            var userId = await GetUserIdAsync();
            
            var updateCategoryCommand = new UpdateCategoryCommand(request.Name, request.Description, 
                request.ExpectedAllocationInPercentage, userId, request.SubCategories, request.Assets);
            await _commandPublisher.SendAsync(updateCategoryCommand);
            
            return WebApiResponse.Accepted();
        }

        private async Task AddUncategorizedAssetsAsync(CategoryResponse categoryResponse, OrdersResponse ordersResponse)
        {
            var categorizedAssets = categoryResponse.GetAssets().Select(asset => asset.AssetId);
            var uncategorizedOrders = ordersResponse.Orders.Where(order => !categorizedAssets.Contains(Guid.Parse(order.AssetId)));
            
            var uncategorizedAssetIds = uncategorizedOrders.Select(order => Guid.Parse(order.AssetId)).Distinct();
            var assetsQuery = _mapper.Map<GetAssetsQuery>(uncategorizedAssetIds);
            var assetsResponse = await _assetClient.GetAllAsync(assetsQuery);

            var assets = assetsResponse.Assets.Select(asset => MapCategoryAsset(asset, uncategorizedOrders));
            if (!assets.Any())
                return;
            
            categoryResponse.SubCategories = categoryResponse.SubCategories.Append(new SubCategoryResponse
            {
                Name = "Uncategorized",
                Description = "All uncategorized assets",
                ExpectedAllocationInPercentage = 0,
                SubCategories = null,
                Assets = assets
            });
        }

        private CategoryAssetResponse MapCategoryAsset(AssetResponse asset, IEnumerable<OrderApi.OrderResponse> orders)
        {
            var assetOrders = orders.Where(order => order.AssetId == asset.Id);
            var result = new CategoryAssetResponse
            {
                Name = asset.Name,
                Symbol = asset.Symbol,
                Type = asset.Type,
                AssetId = Guid.Parse(asset.Id),
                ExpectedAllocationInPercentage = 0,
                Orders = _mapper.Map<List<OrderResponse>>(assetOrders)
            };

            return result;
        }
    }
}