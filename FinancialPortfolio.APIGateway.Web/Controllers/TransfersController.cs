using System;
using System.Threading.Tasks;
using FinancialPortfolio.APIGateway.Contracts.Equity.Commands;
using FinancialPortfolio.APIGateway.Contracts.Equity.Requests;
using FinancialPortfolio.APIGateway.Web.Settings;
using FinancialPortfolio.Messaging;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TransferApi;

namespace FinancialPortfolio.APIGateway.Web.Controllers
{
    [Route("api/transfers")]
    [ApiController]
    [Produces("application/json")]
    public class TransfersController : ControllerBase
    {
        private readonly IMessagePublisher _messagePublisher;
        private readonly ServiceSettings _equityService;

        public TransfersController(IMessagePublisher messagePublisher, IOptions<ServicesSettings> servicesSettingsOptions)
        {
            _messagePublisher = messagePublisher;
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
            var createTransfer = new CreateTransfer(request.Amount, request.Type, request.DateTime);
            await _messagePublisher.PublishAsync(createTransfer);
            
            return Accepted();
        }
    }
}