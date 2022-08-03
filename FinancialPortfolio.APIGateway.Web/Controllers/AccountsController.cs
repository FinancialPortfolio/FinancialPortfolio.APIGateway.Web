using System.Collections.Generic;
using System.Threading.Tasks;
using AccountApi;
using FinancialPortfolio.APIGateway.Contracts.Accounts.Commands;
using FinancialPortfolio.APIGateway.Contracts.Accounts.Requests;
using FinancialPortfolio.APIGateway.Web.Services.Abstraction;
using FinancialPortfolio.CQRS.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinancialPortfolio.APIGateway.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/accounts")]
    [Produces("application/json")]
    public class AccountsController : ApiControllerBase
    {
        private readonly ICommandPublisher _commandPublisher;
        private readonly Account.AccountClient _accountClient;

        public AccountsController(
            ICommandPublisher commandPublisher, IUserInfoService userInfoService, 
            Account.AccountClient accountClient) : base(userInfoService)
        {
            _commandPublisher = commandPublisher;
            _accountClient = accountClient;
        }
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<AccountResponse>>> GetAllAsync()
        {
            var request = new GetAccountsRequest();
            var accountsResponse = await _accountClient.GetAllAsync(request);
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