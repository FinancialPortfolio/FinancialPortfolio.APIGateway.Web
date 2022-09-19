using System;
using System.Threading.Tasks;
using FinancialPortfolio.APIGateway.Contracts.Equity.Commands;
using FinancialPortfolio.APIGateway.Contracts.Integrations.Requests;
using FinancialPortfolio.APIGateway.Contracts.Orders.Commands;
using FinancialPortfolio.APIGateway.Web.Factories;
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
    [Route("api/accounts/{accountId:guid}/integration")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status500InternalServerError)]
    public class IntegrationController : ApiControllerBase
    {
        private readonly ICommandPublisher _commandPublisher;
        private readonly IIntegrationServiceFactory _integrationServiceFactory;

        public IntegrationController(IUserInfoService userInfoService, ICommandPublisher commandPublisher, 
            IIntegrationServiceFactory integrationServiceFactory) : base(userInfoService)
        {
            _commandPublisher = commandPublisher;
            _integrationServiceFactory = integrationServiceFactory;
        }
        
        [HttpPut]
        [ProducesResponseType(typeof(WebApiResponse), StatusCodes.Status202Accepted)]
        public async Task<ActionResult<WebApiResponse>> IntegrateAsync([FromRoute] Guid accountId, [FromForm] IntegrateRequest request)
        {
            var integrationService = _integrationServiceFactory.CreateIntegrationService(request.Source);
    
            var orders = await integrationService.ParseOrdersAsync(request);
            var integrateOrdersCommand = new IntegrateOrdersCommand(accountId, orders);
            await _commandPublisher.SendAsync(integrateOrdersCommand);
            
            var transfers = await integrationService.ParseTransfersAsync(request);
            var integrateTransfersCommand = new IntegrateTransfersCommand(accountId, transfers);
            await _commandPublisher.SendAsync(integrateTransfersCommand);

            return WebApiResponse.Accepted();
        }
    }
}