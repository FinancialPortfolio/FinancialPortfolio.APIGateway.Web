using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FinancialPortfolio.APIGateway.Contracts.Assets.Commands;
using FinancialPortfolio.APIGateway.Contracts.Assets.Requests;
using FinancialPortfolio.CQRS.Commands;
using AssetApi;
using FinancialPortfolio.Infrastructure.WebApi.Models.Response;
using FinancialPortfolio.ProblemDetails.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinancialPortfolio.APIGateway.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/assets")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status500InternalServerError)]
    public class AssetsController : ControllerBase
    {
        private readonly ICommandPublisher _commandPublisher;
        private readonly Asset.AssetClient _assetClient;
        private readonly IMapper _mapper;

        public AssetsController(Asset.AssetClient assetClient, IMapper mapper, ICommandPublisher commandPublisher)
        {
            _assetClient = assetClient;
            _mapper = mapper;
            _commandPublisher = commandPublisher;
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(PaginationWebApiResponse<IEnumerable<AssetResponse>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PaginationWebApiResponse<IEnumerable<AssetResponse>>>> GetAllAsync([FromQuery] GetAssetsRequest request)
        {
            var query = _mapper.Map<GetAssetsQuery>(request);
            var response = await _assetClient.GetAllAsync(query);
            
            return WebApiResponse.Success(response.Assets, response.TotalCount);
        }
        
        [HttpPatch("asset-statistics")]
        [ProducesResponseType(typeof(WebApiResponse), StatusCodes.Status202Accepted)]
        public async Task<ActionResult<WebApiResponse>> FetchAssetStatisticsAsync([FromBody] FetchAssetStatisticsRequest request)
        {
            var fetchAssetStatisticsCommand = new FetchAssetStatisticsCommand(request.Ids);
            await _commandPublisher.SendAsync(fetchAssetStatisticsCommand);
            
            return WebApiResponse.Accepted();
        }
    }
}