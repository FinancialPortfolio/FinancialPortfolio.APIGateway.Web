using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FinancialPortfolio.APIGateway.Contracts.Orders.Commands;
using FinancialPortfolio.APIGateway.Contracts.Orders.Requests;
using FinancialPortfolio.CQRS.Commands;
using FinancialPortfolio.Infrastructure.WebApi.Models.Response;
using FinancialPortfolio.ProblemDetails.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderApi;
using AssetApi;
using FinancialPortfolio.APIGateway.Web.Interfaces;
using OrderResponse = FinancialPortfolio.APIGateway.Contracts.Orders.Responses.OrderResponse;

namespace FinancialPortfolio.APIGateway.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/accounts/{accountId:guid}/orders")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status500InternalServerError)]
    public class OrdersController : ApiControllerBase
    {
        private readonly ICommandPublisher _commandPublisher;
        private readonly Order.OrderClient _orderClient;
        private readonly Asset.AssetClient _assetClient;
        private readonly IMapper _mapper;

        public OrdersController(ICommandPublisher commandPublisher, Order.OrderClient orderClient, 
            Asset.AssetClient assetClient, IMapper mapper, IUserInfoService userInfoService) : base(userInfoService)
        {
            _commandPublisher = commandPublisher;
            _orderClient = orderClient;
            _assetClient = assetClient;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginationWebApiResponse<IEnumerable<OrderResponse>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PaginationWebApiResponse<IEnumerable<OrderResponse>>>> GetAllAsync(
            [FromRoute] Guid accountId, [FromQuery] GetOrdersRequest request)
        {
            await ValidateUserAccountAsync(accountId);
            
            var ordersQuery = _mapper.Map<GetOrdersQuery>((request, accountId));
            var ordersResponse = await _orderClient.GetAllAsync(ordersQuery);

            var assetIds = ordersResponse.Orders.Select(order => Guid.Parse(order.AssetId));
            var assetsQuery = _mapper.Map<GetAssetsQuery>(assetIds);
            var assetsResponse = await _assetClient.GetAllAsync(assetsQuery);
            
            var orders = _mapper.Map<IEnumerable<OrderResponse>>((ordersResponse.Orders, assetsResponse.Assets));
            
            return WebApiResponse.Success(orders, ordersResponse.TotalCount);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(WebApiResponse<OrderResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WebApiResponse<OrderResponse>>> GetAsync([FromRoute] Guid accountId, [FromRoute] Guid id)
        {
            await ValidateUserAccountAsync(accountId);
            
            var ordersQuery = new GetOrderQuery { Id = id.ToString(), AccountId = accountId.ToString()};
            var order = await _orderClient.GetAsync(ordersQuery);
            
            var assetQuery = new GetAssetQuery { Id = order.AssetId };
            var asset = await _assetClient.GetAsync(assetQuery);

            var response = _mapper.Map<OrderResponse>((order, asset));

            return WebApiResponse.Success(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(WebApiResponse), StatusCodes.Status202Accepted)]
        public async Task<ActionResult<WebApiResponse>> CreateAsync([FromRoute] Guid accountId, [FromBody] CreateOrderRequest request)
        {
            await ValidateUserAccountAsync(accountId);
            
            var createOrderCommand = new CreateOrderCommand(request.Type, request.Amount, 
                request.Price, request.DateTime, request.Commission, request.AssetId, accountId);
            await _commandPublisher.SendAsync(createOrderCommand);

            return WebApiResponse.Accepted();
        }
        
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(WebApiResponse), StatusCodes.Status202Accepted)]
        public async Task<ActionResult<WebApiResponse>> UpdateAsync([FromRoute] Guid accountId, [FromRoute] Guid id, [FromBody] UpdateOrderRequest request)
        {
            await ValidateUserAccountAsync(accountId);
            
            var updateOrderCommand = new UpdateOrderCommand(id, request.Type, request.Amount, 
                request.Price, request.DateTime, request.Commission, request.AssetId, accountId);
            await _commandPublisher.SendAsync(updateOrderCommand);
            
            return WebApiResponse.Accepted();
        }
        
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(WebApiResponse), StatusCodes.Status202Accepted)]
        public async Task<ActionResult<WebApiResponse>> DeleteAsync([FromRoute] Guid accountId, [FromRoute] Guid id)
        {
            await ValidateUserAccountAsync(accountId);
            
            var deleteOrderCommand = new DeleteOrderCommand(id, accountId);
            await _commandPublisher.SendAsync(deleteOrderCommand);
            
            return WebApiResponse.Accepted();
        }
    }
}