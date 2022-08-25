using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AssetApi;
using AutoMapper;
using FinancialPortfolio.APIGateway.Contracts.Assets.Commands;
using FinancialPortfolio.APIGateway.Contracts.Assets.Requests;
using FinancialPortfolio.CQRS.Commands;
using FinancialPortfolio.Infrastructure.WebApi.Models.Response;
using FinancialPortfolio.ProblemDetails.WebApi.Models;
using FinancialPortfolio.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinancialPortfolio.APIGateway.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/assets")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status500InternalServerError)]
    public class AssetsController : ControllerBase
    {
        private readonly ICommandPublisher _commandPublisher;
        private readonly Asset.AssetClient _assetClient;
        private readonly IMapper _mapper;

        public AssetsController(ICommandPublisher commandPublisher, 
            Asset.AssetClient assetClient, IMapper mapper)
        {
            _commandPublisher = commandPublisher;
            _assetClient = assetClient;
            _mapper = mapper;
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(WebApiResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<WebApiResponse<IEnumerable<AssetResponse>>>> GetAllAsync([FromBody] SearchOptions search)
        {
            var grpcSearch = _mapper.Map<SearchLibrary.SearchOptions>(search);
            var request = new GetAssetsQuery { Search = grpcSearch };
            var response = await _assetClient.GetAllAsync(request);
            
            return WebApiResponse.Success(response.Assets);
        }
        
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(WebApiResponse<AssetResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WebApiResponse<AssetResponse>>> GetAsync([FromRoute] Guid id)
        {
            var request = new GetAssetQuery { Id = id.ToString() };
            var response = await _assetClient.GetAsync(request);
            
            return WebApiResponse.Success(response);
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(WebApiResponse), StatusCodes.Status202Accepted)]
        public async Task<ActionResult<WebApiResponse>> CreateAsync([FromBody] CreateAssetRequest request)
        {
            var createAssetCommand = new CreateAssetCommand(request.Symbol, request.Name, request.Type);
            await _commandPublisher.SendAsync(createAssetCommand);
            
            return WebApiResponse.Accepted();
        }
    }
}