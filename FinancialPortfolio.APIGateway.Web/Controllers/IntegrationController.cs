using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FinancialPortfolio.APIGateway.Contracts.Equity.Commands;
using FinancialPortfolio.APIGateway.Contracts.Integrations.Requests;
using FinancialPortfolio.APIGateway.Contracts.Integrations.Responses;
using FinancialPortfolio.APIGateway.Contracts.Orders.Commands;
using FinancialPortfolio.APIGateway.Web.Factories;
using FinancialPortfolio.APIGateway.Web.Interfaces;
using FinancialPortfolio.CQRS.Commands;
using FinancialPortfolio.Infrastructure.WebApi.Models.Response;
using FinancialPortfolio.ProblemDetails.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AssetApi;

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
        private readonly Asset.AssetClient _assetClient;
        private readonly IMapper _mapper;

        public IntegrationController(IUserInfoService userInfoService, ICommandPublisher commandPublisher, 
            IIntegrationServiceFactory integrationServiceFactory, Asset.AssetClient assetClient, IMapper mapper) : base(userInfoService)
        {
            _commandPublisher = commandPublisher;
            _integrationServiceFactory = integrationServiceFactory;
            _assetClient = assetClient;
            _mapper = mapper;
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

        [HttpPost("validate")]
        [ProducesResponseType(typeof(WebApiResponse<IntegrationFileValidationResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<WebApiResponse<IntegrationFileValidationResponse>>> ValidateAsync([FromRoute] Guid accountId, [FromForm] IntegrateRequest request)
        {
            var integrationService = _integrationServiceFactory.CreateIntegrationService(request.Source);
    
            var orders = await integrationService.ParseOrdersAsync(request);

            var validationResponse = await ValidateIntegrationFileAsync(orders);

            return WebApiResponse.Success(validationResponse);
        }

        private async Task<IntegrationFileValidationResponse> ValidateIntegrationFileAsync(IEnumerable<IntegrateOrderCommand> orders)
        {
            var invalidOrders = new List<InvalidOrderResponse>();
            var assets = await GetAssetsAsync(orders);
            
            foreach (var order in orders)
            {
                var asset = assets.FirstOrDefault(asset => Compare(order, asset));
                if (asset is not null)
                    continue;

                var invalidOrder = new InvalidOrderResponse(order.Symbol, order.Exchange, order.Currency, order.DateTime, order.Amount);
                invalidOrders.Add(invalidOrder);
            }
            
            return new IntegrationFileValidationResponse(invalidOrders);
        }

        private static bool Compare(IntegrateOrderCommand order, AssetResponse asset)
        {
            if (order.Symbol != asset.Symbol)
                return false;
            
            if (order.Currency is not null && order.Currency != asset.Currency)
                return false;
            
            if (order.Exchange is not null && order.Exchange != asset.Exchange)
                return false;

            return true;
        }

        private async Task<IEnumerable<AssetResponse>> GetAssetsAsync(IEnumerable<IntegrateOrderCommand> orders)
        {
            var symbols = orders.Select(order => order.Symbol);
            var assetsQuery = _mapper.Map<GetAssetsQuery>(symbols);
            var assetsResponse = await _assetClient.GetAllAsync(assetsQuery);
            
            return assetsResponse.Assets;
        }
    }
}