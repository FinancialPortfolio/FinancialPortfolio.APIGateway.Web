using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AccountApi;
using AutoMapper;
using FinancialPortfolio.APIGateway.Contracts.Accounts.Commands;
using FinancialPortfolio.APIGateway.Contracts.Accounts.Requests;
using FinancialPortfolio.APIGateway.Web.Services.Abstraction;
using FinancialPortfolio.CQRS.Commands;
using Microsoft.AspNetCore.Authorization;
using FinancialPortfolio.Search;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinancialPortfolio.APIGateway.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/accounts")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class AccountsController : ApiControllerBase
    {
        private readonly ICommandPublisher _commandPublisher;
        private readonly Account.AccountClient _accountClient;
        private readonly IMapper _mapper;

        public AccountsController(
            ICommandPublisher commandPublisher, IUserInfoService userInfoService, 
            Account.AccountClient accountClient, IMapper mapper) : base(userInfoService)
        {
            _commandPublisher = commandPublisher;
            _accountClient = accountClient;
            _mapper = mapper;
        }
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<AccountResponse>>> GetAllAsync([FromBody] SearchOptions search)
        {
            var grpcSearch = _mapper.Map<SearchLibrary.SearchOptions>(search);
            var request = new GetAccountsRequest { Search = grpcSearch };
            var accountsResponse = await _accountClient.GetAllAsync(request);
            return Ok(accountsResponse.Accounts);
        }
        
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AccountResponse>> GetAsync([FromRoute] Guid id)
        {
            var request = new GetAccountRequest { Id = id.ToString() };
            var accountResponse = await _accountClient.GetAsync(request);
            return Ok(accountResponse);
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<ActionResult> CreateAsync([FromBody] CreateAccountRequest request)
        {
            var userId = await GetUserIdAsync();
            
            var createAccountCommand = new CreateAccountCommand(request.Name, request.Description, userId);
            await _commandPublisher.SendAsync(createAccountCommand);
            
            return Accepted();
        }
        
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<ActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateAccountRequest request)
        {
            var userId = await GetUserIdAsync();
            
            var updateAccountCommand = new UpdateAccountCommand(id, request.Name, request.Description, userId);
            await _commandPublisher.SendAsync(updateAccountCommand);
            
            return Accepted();
        }
        
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<ActionResult> DeleteAsync([FromRoute] Guid id)
        {
            var userId = await GetUserIdAsync();
            
            var deleteAccountCommand = new DeleteAccountCommand(id, userId);
            await _commandPublisher.SendAsync(deleteAccountCommand);
            
            return Accepted();
        }
    }
}