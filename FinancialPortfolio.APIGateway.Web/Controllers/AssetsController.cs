using System.Collections.Generic;
using System.Threading.Tasks;
using AssetApi;
using FinancialPortfolio.APIGateway.Contracts.Assets.Commands;
using FinancialPortfolio.APIGateway.Contracts.Assets.Requests;
using FinancialPortfolio.CQRS.Commands;
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

        public AssetsController(ICommandPublisher commandPublisher, Asset.AssetClient assetClient)
        {
            _commandPublisher = commandPublisher;
            _assetClient = assetClient;
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(WebApiResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<AssetResponse>>> GetAllAsync()
        {
            var request = new GetAssetsRequest();
            var assetsResponse = await _assetClient.GetAllAsync(request);
            return WebApiResponse.Success(assetsResponse.Assets);
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(WebApiResponse), StatusCodes.Status202Accepted)]
        public async Task<ActionResult> CreateAsync([FromBody] CreateAssetRequest request)
        {
            var createAssetCommand = new CreateAssetCommand(request.Symbol, request.Name, request.Type);
            await _commandPublisher.SendAsync(createAssetCommand);
            
            return WebApiResponse.Accepted();
        }
    }
}