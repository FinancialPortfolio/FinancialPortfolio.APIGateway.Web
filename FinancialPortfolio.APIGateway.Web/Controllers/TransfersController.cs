using System.Collections.Generic;
using System.Threading.Tasks;
using FinancialPortfolio.APIGateway.Contracts.Equity.Commands;
using FinancialPortfolio.APIGateway.Contracts.Equity.Requests;
using FinancialPortfolio.CQRS.Commands;
using FinancialPortfolio.Infrastructure.WebApi.Models.Response;
using FinancialPortfolio.ProblemDetails.WebApi.Models;
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
    [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status500InternalServerError)]
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
        [ProducesResponseType(typeof(WebApiResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TransferResponse>>> GetAllAsync()
        {
            var request = new GetTransfersRequest();
            var transfersResponse = await _transferClient.GetAllAsync(request);
            return WebApiResponse.Success(transfersResponse.Transfers);
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(WebApiResponse), StatusCodes.Status202Accepted)]
        public async Task<ActionResult> CreateAsync([FromBody] CreateTransferRequest request)
        {
            var createTransferCommand = new CreateTransferCommand(request.Amount, request.Type, request.DateTime, request.AccountId);
            await _commandPublisher.SendAsync(createTransferCommand);
            
            return WebApiResponse.Accepted();
        }
    }
}