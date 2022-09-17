using System.Threading.Tasks;
using FinancialPortfolio.APIGateway.Contracts.Integrations.Requests;
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
    [Route("api/integration")]
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
        
        [HttpPost]
        [ProducesResponseType(typeof(WebApiResponse), StatusCodes.Status202Accepted)]
        public async Task<ActionResult<WebApiResponse>> IntegrateAsync([FromForm] IntegrateRequest request)
        {
            var integrationService = _integrationServiceFactory.CreateIntegrationService(request.Source);
            var integrationCommands = integrationService.Parse(request);

            await _commandPublisher.SendAsync(integrationCommands.IntegrateOrdersCommand);
            
            await _commandPublisher.SendAsync(integrationCommands.IntegrateTransfersCommand);

            return WebApiResponse.Accepted();
        }
    }
}