using System.Collections.Generic;
using System.Threading.Tasks;
using FinancialPortfolio.APIGateway.Contracts.Equity.Commands;
using FinancialPortfolio.APIGateway.Contracts.Equity.Requests;
using FinancialPortfolio.CQRS.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TransferApi;

namespace FinancialPortfolio.APIGateway.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/transfers")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class TransfersController : ControllerBase
    {
        private readonly ICommandPublisher _commandPublisher;
        private readonly Transfer.TransferClient _transferClient;

        public TransfersController(ICommandPublisher commandPublisher, Transfer.TransferClient transferClient)
        {
            _commandPublisher = commandPublisher;
            _transferClient = transferClient;
        }
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TransferResponse>>> GetAllAsync()
        {
            var request = new GetTransfersRequest();
            var transfersResponse = await _transferClient.GetAllAsync(request);
            return Ok(transfersResponse.Transfers);
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<ActionResult> CreateAsync([FromBody] CreateTransferRequest request)
        {
            var createTransferCommand = new CreateTransferCommand(request.Amount, request.Type, request.DateTime, request.AccountId);
            await _commandPublisher.SendAsync(createTransferCommand);
            
            return Accepted();
        }
    }
}