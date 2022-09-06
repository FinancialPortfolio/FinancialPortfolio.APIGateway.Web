using System;
using System.Collections.Generic;
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
        private readonly IMapper _mapper;

        public OrdersController(ICommandPublisher commandPublisher,
            Order.OrderClient orderClient, IMapper mapper)
        {
            _commandPublisher = commandPublisher;
            _orderClient = orderClient;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginationWebApiResponse<IEnumerable<OrderResponse>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PaginationWebApiResponse<IEnumerable<OrderResponse>>>> GetAllAsync(
            [FromRoute] Guid accountId, [FromQuery] GetOrdersRequest request)
        {
            var query = _mapper.Map<GetOrdersQuery>((request, accountId));
            var response = await _orderClient.GetAllAsync(query);

            return WebApiResponse.Success(response.Orders, response.TotalCount);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(WebApiResponse<OrderResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WebApiResponse<OrderResponse>>> GetAsync([FromRoute] Guid accountId, [FromRoute] Guid id)
        {
            var query = new GetOrderQuery { Id = id.ToString() };
            var response = await _orderClient.GetAsync(query);

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