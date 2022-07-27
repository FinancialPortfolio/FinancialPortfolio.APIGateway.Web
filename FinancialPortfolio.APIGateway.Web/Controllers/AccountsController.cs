using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AccountApi;
using FinancialPortfolio.APIGateway.Contracts.Accounts.Commands;
using FinancialPortfolio.APIGateway.Contracts.Accounts.Requests;
using FinancialPortfolio.APIGateway.Web.Models.Settings;
using FinancialPortfolio.APIGateway.Web.Services.Abstraction;
using FinancialPortfolio.CQRS.Commands;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FinancialPortfolio.APIGateway.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/accounts")]
    [Produces("application/json")]
    public class AccountsController : ApiControllerBase
    {
        private readonly ICommandPublisher _commandPublisher;
        private readonly ServiceSettings _accountsService;

        public AccountsController(
            ICommandPublisher commandPublisher, ServicesSettings servicesSettings, 
            IUserInfoService userInfoService) : base(userInfoService)
        {
            _commandPublisher = commandPublisher;
            _accountsService = servicesSettings.AccountsService;
        }
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<AccountResponse>>> GetAllAsync()
        {
            var channel = GrpcChannel.ForAddress(_accountsService.GrpcUrl);
            var client = new Account.AccountClient(channel);

            var request = new GetAccountsRequest();
            var accountsResponse = await client.GetAllAsync(request);
            return Ok(accountsResponse.Accounts);
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Create([FromBody] CreateAccountRequest request)
        {
            var userId = await GetUserIdAsync();
            
            var createAccountCommand = new CreateAccountCommand(request.Name, request.Description, userId);
            await _commandPublisher.SendAsync(createAccountCommand);
            
            return Accepted();
        }
    }
}