using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FinancialPortfolio.APIGateway.Contracts.Equity.Commands;
using FinancialPortfolio.APIGateway.Contracts.Equity.Requests;
using FinancialPortfolio.CQRS.Commands;
using FinancialPortfolio.Infrastructure.WebApi.Models.Response;
using FinancialPortfolio.ProblemDetails.WebApi.Models;
using FinancialPortfolio.Search;
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
        private readonly IMapper _mapper;
        
        public TransfersController(ICommandPublisher commandPublisher, 
            Transfer.TransferClient transferClient, IMapper mapper)
        {
            _commandPublisher = commandPublisher;
            _transferClient = transferClient;
            _mapper = mapper;
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(WebApiResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<WebApiResponse<IEnumerable<TransferResponse>>>> GetAllAsync()
        {
            var request = new GetTransfersQuery();
            var response = await _transferClient.GetAllAsync(request);
            
            return WebApiResponse.Success(response.Transfers);
        }
        
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(WebApiResponse<TransferResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WebApiResponse<TransferResponse>>> GetAsync([FromRoute] Guid id)
        {
            var request = new GetTransferQuery { Id = id.ToString() };
            var response = await _transferClient.GetAsync(request);
            
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