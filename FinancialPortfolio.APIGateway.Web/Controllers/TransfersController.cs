using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinancialPortfolio.APIGateway.Contracts.Assets.Requests;
using FinancialPortfolio.APIGateway.Contracts.Equity.Commands;
using FinancialPortfolio.APIGateway.Contracts.Equity.Requests;
using FinancialPortfolio.CQRS.Commands;
using FinancialPortfolio.Infrastructure.WebApi.Models.Response;
using FinancialPortfolio.ProblemDetails.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SearchLibrary;
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
        
        public TransfersController(ICommandPublisher commandPublisher, 
            Transfer.TransferClient transferClient)
        {
            _commandPublisher = commandPublisher;
            _transferClient = transferClient;
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(PaginationWebApiResponse<IEnumerable<TransferResponse>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PaginationWebApiResponse<IEnumerable<TransferResponse>>>> GetAllAsync([FromQuery] GetTransfersRequest request)
        {
            var query = new GetTransfersQuery
            {
                Search = new SearchOptions
                {
                    PaginationOptions = new PaginationOptions
                    {
                        PageNumber = request.PageNumber,
                        PageSize = request.PageSize
                    }
                }
            };
            var response = await _transferClient.GetAllAsync(query);
            
            return WebApiResponse.Success(response.Transfers, response.TotalCount);
        }
        
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(WebApiResponse<TransferResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WebApiResponse<TransferResponse>>> GetAsync([FromRoute] Guid id)
        {
            var query = new GetTransferQuery { Id = id.ToString() };
            var response = await _transferClient.GetAsync(query);
            
            return WebApiResponse.Success(response);
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(WebApiResponse), StatusCodes.Status202Accepted)]
        public async Task<ActionResult<WebApiResponse>> CreateAsync([FromBody] CreateTransferRequest request)
        {
            var createTransferCommand = new CreateTransferCommand(request.Amount, request.Type, request.DateTime, request.AccountId);
            await _commandPublisher.SendAsync(createTransferCommand);
            
            return WebApiResponse.Accepted();
        }
    }
}