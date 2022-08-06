using System.Collections.Generic;
using System.Threading.Tasks;
using AssetApi;
using FinancialPortfolio.APIGateway.Contracts.Assets.Commands;
using FinancialPortfolio.APIGateway.Contracts.Assets.Requests;
using FinancialPortfolio.CQRS.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinancialPortfolio.APIGateway.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/assets")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<AssetResponse>>> GetAllAsync()
        {
            var request = new GetAssetsRequest();
            var assetsResponse = await _assetClient.GetAllAsync(request);
            return Ok(assetsResponse.Assets);
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<ActionResult> CreateAsync([FromBody] CreateAssetRequest request)
        {
            var createAssetCommand = new CreateAssetCommand(request.Symbol, request.Name, request.Type);
            await _commandPublisher.SendAsync(createAssetCommand);
            
            return Accepted();
        }
    }
}