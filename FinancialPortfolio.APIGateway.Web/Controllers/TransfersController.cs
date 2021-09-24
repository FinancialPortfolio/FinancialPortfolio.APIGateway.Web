using System.Threading.Tasks;
using FinancialPortfolio.APIGateway.Contracts.Equity.Commands;
using FinancialPortfolio.APIGateway.Contracts.Equity.Requests;
using FinancialPortfolio.APIGateway.Web.Models.Settings;
using FinancialPortfolio.CQRS.Publishers;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TransferApi;

namespace FinancialPortfolio.APIGateway.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/transfers")]
    [Produces("application/json")]
    public class TransfersController : ControllerBase
    {
        private readonly ICommandPublisher _commandPublisher;
        private readonly ServiceSettings _equityService;

        public TransfersController(ICommandPublisher commandPublisher, IOptions<ServicesSettings> servicesSettingsOptions)
        {
            _commandPublisher = commandPublisher;
            _equityService = servicesSettingsOptions.Value.EquityService;
        }
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAllAsync()
        {
            var channel = GrpcChannel.ForAddress(_equityService.GrpcUrl);
            var client = new Transfer.TransferClient(channel);

            var request = new GetTransfersRequest();
            var transfers = await client.GetAllAsync(request);
            return Ok(transfers);
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Create([FromBody] CreateTransferRequest request)
        {
            var createTransferCommand = new CreateTransferCommand(request.Amount, request.Type, request.DateTime);
            await _commandPublisher.SendAsync(createTransferCommand);
            
            return Accepted();
        }
    }
}