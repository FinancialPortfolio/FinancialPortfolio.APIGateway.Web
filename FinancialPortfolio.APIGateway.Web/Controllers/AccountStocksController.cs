using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FinancialPortfolio.APIGateway.Contracts.Assets.Requests;
using FinancialPortfolio.APIGateway.Contracts.Assets.Responses;
using FinancialPortfolio.Infrastructure.WebApi.Models.Response;
using FinancialPortfolio.ProblemDetails.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderApi;
using AssetApi;

namespace FinancialPortfolio.APIGateway.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/accounts/{accountId:guid}/assets")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status500InternalServerError)]
    public class AccountAssetsController : ControllerBase
    {
        private readonly Order.OrderClient _orderClient;
        private readonly Asset.AssetClient _assetClient;
        private readonly IMapper _mapper;

        public AccountAssetsController(Order.OrderClient orderClient, Asset.AssetClient assetClient, IMapper mapper)
        {
            _orderClient = orderClient;
            _assetClient = assetClient;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(WebApiResponse<IEnumerable<AccountAssetResponse>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<WebApiResponse<IEnumerable<AccountAssetResponse>>>> GetAllAsync([FromRoute] Guid accountId, [FromQuery] GetAccountAssetsRequest request)
        {
            var ordersQuery = _mapper.Map<GetOrdersQuery>((accountId, "AccountId"));
            var ordersResponse = await _orderClient.GetAllAsync(ordersQuery);

            var assetIds = ordersResponse.Orders.Select(order => Guid.Parse(order.AssetId));
            var assetsQuery = _mapper.Map<GetAssetsQuery>((assetIds, request));
            var assetsResponse = await _assetClient.GetAllAsync(assetsQuery);
            
            var accountAssets = _mapper.Map<IEnumerable<AccountAssetResponse>>((assetsResponse.Assets, ordersResponse.Orders));
            var availableAccountAssets = accountAssets.Where(a => a.NumberOfShares > 0);
            
            return WebApiResponse.Success(availableAccountAssets);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(WebApiResponse<AccountAssetResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WebApiResponse<AccountAssetResponse>>> GetAsync([FromRoute] Guid accountId, [FromRoute] Guid id)
        {
            var assetQuery = new GetAssetQuery { Id = id.ToString() };
            var asset = await _assetClient.GetAsync(assetQuery);
            
            var ordersQuery = _mapper.Map<GetOrdersQuery>(asset);
            var orders = await _orderClient.GetAllAsync(ordersQuery);

            var response = _mapper.Map<AccountAssetResponse>((asset, orders));
            
            return WebApiResponse.Success(response);
        }
    }
}