using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FinancialPortfolio.APIGateway.Contracts.Assets.Commands;
using FinancialPortfolio.APIGateway.Contracts.Assets.Requests;
using FinancialPortfolio.CQRS.Commands;
using StockApi;
using FinancialPortfolio.Infrastructure.WebApi.Models.Response;
using FinancialPortfolio.ProblemDetails.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinancialPortfolio.APIGateway.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/stocks")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status500InternalServerError)]
    public class StocksController : ControllerBase
    {
        private readonly ICommandPublisher _commandPublisher;
        private readonly Stock.StockClient _stockClient;
        private readonly IMapper _mapper;

        public StocksController(Stock.StockClient stockClient, IMapper mapper, ICommandPublisher commandPublisher)
        {
            _stockClient = stockClient;
            _mapper = mapper;
            _commandPublisher = commandPublisher;
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(PaginationWebApiResponse<IEnumerable<StockResponse>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PaginationWebApiResponse<IEnumerable<StockResponse>>>> GetAllAsync([FromQuery] GetStocksRequest request)
        {
            var query = _mapper.Map<GetStocksQuery>(request);
            var response = await _stockClient.GetAllAsync(query);
            
            return WebApiResponse.Success(response.Stocks, response.TotalCount);
        }
        
        [HttpPatch("asset-statistics")]
        [ProducesResponseType(typeof(WebApiResponse), StatusCodes.Status202Accepted)]
        public async Task<ActionResult<WebApiResponse>> FetchAssetStatisticsAsync([FromBody] FetchAssetStatisticsRequest request)
        {
            var fetchAssetStatisticsCommand = new FetchAssetStatisticsCommand(request.Symbols);
            await _commandPublisher.SendAsync(fetchAssetStatisticsCommand);
            
            return WebApiResponse.Accepted();
        }
    }
}