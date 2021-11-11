using System.Collections.Generic;
using System.Threading.Tasks;
using AssetApi;
using FinancialPortfolio.APIGateway.Contracts.Assets.Commands;
using FinancialPortfolio.APIGateway.Contracts.Assets.Requests;
using FinancialPortfolio.APIGateway.Web.Models.Settings;
using FinancialPortfolio.CQRS.Commands;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinancialPortfolio.APIGateway.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/assets")]
    [Produces("application/json")]
    public class AssetsController : ControllerBase
    {
        private readonly ICommandPublisher _commandPublisher;
        private readonly ServiceSettings _assetsService;

        public AssetsController(ICommandPublisher commandPublisher, ServicesSettings servicesSettings)
        {
            _commandPublisher = commandPublisher;
            _assetsService = servicesSettings.AssetsService;
        }
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<AssetResponse>>> GetAllAsync()
        {
            var channel = GrpcChannel.ForAddress(_assetsService.GrpcUrl);
            var client = new Asset.AssetClient(channel);

            var request = new GetAssetsRequest();
            var assetsResponse = await client.GetAllAsync(request);
            return Ok(assetsResponse.Assets);
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Create([FromBody] CreateAssetRequest request)
        {
            var createAssetCommand = new CreateAssetCommand(request.Symbol, request.Name, request.Type);
            await _commandPublisher.SendAsync(createAssetCommand);
            
            return Accepted();
        }
    }
}