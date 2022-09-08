using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FinancialPortfolio.APIGateway.Contracts.Assets.Responses;
using FinancialPortfolio.APIGateway.Contracts.Orders.Requests;
using FinancialPortfolio.Infrastructure.WebApi.Models.Response;
using FinancialPortfolio.ProblemDetails.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderApi;
using StockApi;

namespace FinancialPortfolio.APIGateway.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/accounts/{accountId:guid}/stocks")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status500InternalServerError)]
    public class AccountStocksController : ControllerBase
    {
        private readonly Order.OrderClient _orderClient;
        private readonly Stock.StockClient _stockClient;
        private readonly IMapper _mapper;

        public AccountStocksController(Order.OrderClient orderClient, Stock.StockClient stockClient, IMapper mapper)
        {
            _orderClient = orderClient;
            _stockClient = stockClient;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginationWebApiResponse<IEnumerable<AccountStockResponse>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PaginationWebApiResponse<IEnumerable<AccountStockResponse>>>> GetAllAsync([FromRoute] Guid accountId)
        {
            var ordersQuery = _mapper.Map<GetOrdersQuery>(accountId);
            var ordersResponse = await _orderClient.GetAllAsync(ordersQuery);

            var stockIds = ordersResponse.Orders.Select(order => order.AssetId);
            var stocksQuery = _mapper.Map<GetStocksQuery>(stockIds);
            var stocksResponse = await _stockClient.GetAllAsync(stocksQuery);
            
            var accountStocks = _mapper.Map<IEnumerable<AccountStockResponse>>((stocksResponse.Stocks, ordersResponse.Orders));

            return WebApiResponse.Success(accountStocks, ordersResponse.TotalCount);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(WebApiResponse<AccountStockResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WebApiResponse<AccountStockResponse>>> GetAsync([FromRoute] Guid accountId, [FromRoute] Guid id)
        {
            var stockQuery = new GetStockQuery { Id = id.ToString() };
            var stock = await _stockClient.GetAsync(stockQuery);
            
            var ordersQuery = _mapper.Map<GetOrdersQuery>(stock);
            var orders = await _orderClient.GetAllAsync(ordersQuery);

            var response = _mapper.Map<AccountStockResponse>((stock, orders));
            
            return WebApiResponse.Success(response);
        }
    }
}