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
using StockApi;
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
    public class OrdersController : ControllerBase
    {
        private readonly ICommandPublisher _commandPublisher;
        private readonly Order.OrderClient _orderClient;
        private readonly Stock.StockClient _stockClient;
        private readonly IMapper _mapper;

        public OrdersController(ICommandPublisher commandPublisher,
            Order.OrderClient orderClient, Stock.StockClient stockClient, IMapper mapper)
        {
            _commandPublisher = commandPublisher;
            _orderClient = orderClient;
            _stockClient = stockClient;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginationWebApiResponse<IEnumerable<OrderResponse>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PaginationWebApiResponse<IEnumerable<OrderResponse>>>> GetAllAsync(
            [FromRoute] Guid accountId, [FromQuery] GetOrdersRequest request)
        {
            var ordersQuery = _mapper.Map<GetOrdersQuery>((request, accountId));
            var ordersResponse = await _orderClient.GetAllAsync(ordersQuery);

            var stockIds = ordersResponse.Orders.Select(order => order.AssetId);
            var stocksQuery = _mapper.Map<GetStocksQuery>(stockIds);
            var stocksResponse = await _stockClient.GetAllAsync(stocksQuery);
            
            var orders = _mapper.Map<IEnumerable<OrderResponse>>((ordersResponse.Orders, stocksResponse.Stocks));
            
            return WebApiResponse.Success(orders, ordersResponse.TotalCount);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(WebApiResponse<OrderResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WebApiResponse<OrderResponse>>> GetAsync([FromRoute] Guid accountId, [FromRoute] Guid id)
        {
            var ordersQuery = new GetOrderQuery { Id = id.ToString() };
            var order = await _orderClient.GetAsync(ordersQuery);
            
            var stockQuery = new GetStockQuery { Id = order.AssetId };
            var stock = await _stockClient.GetAsync(stockQuery);

            var response = _mapper.Map<OrderResponse>((order, stock));

            return WebApiResponse.Success(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(WebApiResponse), StatusCodes.Status202Accepted)]
        public async Task<ActionResult<WebApiResponse>> CreateAsync([FromRoute] Guid accountId, [FromBody] CreateOrderRequest request)
        {
            var createOrderCommand = new CreateOrderCommand(request.Type, request.Amount, 
                request.Price, request.DateTime, request.Commission, request.AssetId, accountId);
            await _commandPublisher.SendAsync(createOrderCommand);

            return WebApiResponse.Accepted();
        }
        
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(WebApiResponse), StatusCodes.Status202Accepted)]
        public async Task<ActionResult<WebApiResponse>> UpdateAsync([FromRoute] Guid accountId, [FromRoute] Guid id, [FromBody] UpdateOrderRequest request)
        {
            var updateOrderCommand = new UpdateOrderCommand(id, request.Type, request.Amount, 
                request.Price, request.DateTime, request.Commission, request.AssetId, accountId);
            await _commandPublisher.SendAsync(updateOrderCommand);
            
            return WebApiResponse.Accepted();
        }
        
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(WebApiResponse), StatusCodes.Status202Accepted)]
        public async Task<ActionResult<WebApiResponse>> DeleteAsync([FromRoute] Guid accountId, [FromRoute] Guid id)
        {
            var deleteOrderCommand = new DeleteOrderCommand(id, accountId);
            await _commandPublisher.SendAsync(deleteOrderCommand);
            
            return WebApiResponse.Accepted();
        }
    }
}