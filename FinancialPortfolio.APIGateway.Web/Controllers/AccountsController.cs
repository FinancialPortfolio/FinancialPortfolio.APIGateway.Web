using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AccountApi;
using AutoMapper;
using FinancialPortfolio.APIGateway.Contracts.Accounts.Commands;
using FinancialPortfolio.APIGateway.Contracts.Accounts.Requests;
using FinancialPortfolio.APIGateway.Web.Interfaces;
using FinancialPortfolio.CQRS.Commands;
using FinancialPortfolio.Infrastructure.WebApi.Models.Response;
using FinancialPortfolio.ProblemDetails.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinancialPortfolio.APIGateway.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/accounts")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status500InternalServerError)]
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
        [ProducesResponseType(typeof(PaginationWebApiResponse<IEnumerable<AccountResponse>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PaginationWebApiResponse<IEnumerable<AccountResponse>>>> GetAllAsync()
        {
            var userId = await GetUserIdAsync();
            var query = _mapper.Map<GetAccountsQuery>(userId);
            var response = await _accountClient.GetAllAsync(query);

            return WebApiResponse.Success(response.Accounts, response.TotalCount);
        }
        
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(WebApiResponse<AccountResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WebApiResponse<AccountResponse>>> GetAsync([FromRoute] Guid id)
        {
            await ValidateUserAccountAsync(id);
            
            var query = new GetAccountQuery { Id = id.ToString() };
            var response = await _accountClient.GetAsync(query);
            
            return WebApiResponse.Success(response);
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(WebApiResponse), StatusCodes.Status202Accepted)]
        public async Task<ActionResult<WebApiResponse>> CreateAsync([FromBody] CreateAccountRequest request)
        {
            var userId = await GetUserIdAsync();
            
            var createAccountCommand = new CreateAccountCommand(request.Name, request.Description, userId);
            await _commandPublisher.SendAsync(createAccountCommand);

            return WebApiResponse.Accepted();
        }
        
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(WebApiResponse), StatusCodes.Status202Accepted)]
        public async Task<ActionResult<WebApiResponse>> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateAccountRequest request)
        {
            await ValidateUserAccountAsync(id);
            
            var updateAccountCommand = new UpdateAccountCommand(id, request.Name, request.Description);
            await _commandPublisher.SendAsync(updateAccountCommand);
            
            return WebApiResponse.Accepted();
        }
        
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(WebApiResponse), StatusCodes.Status202Accepted)]
        public async Task<ActionResult<WebApiResponse>> DeleteAsync([FromRoute] Guid id)
        {
            await ValidateUserAccountAsync(id);
            
            var deleteAccountCommand = new DeleteAccountCommand(id);
            await _commandPublisher.SendAsync(deleteAccountCommand);
            
            return WebApiResponse.Accepted();
        }
    }
}