using System.Threading.Tasks;
using AutoMapper;
using CategoryApi;
using FinancialPortfolio.APIGateway.Contracts.Categories.Commands;
using FinancialPortfolio.APIGateway.Contracts.Categories.Requests;
using FinancialPortfolio.APIGateway.Web.Interfaces;
using FinancialPortfolio.CQRS.Commands;
using FinancialPortfolio.Infrastructure.WebApi.Models.Response;
using FinancialPortfolio.ProblemDetails.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderApi;
using CategoryResponse = FinancialPortfolio.APIGateway.Contracts.Categories.Responses.CategoryResponse;

namespace FinancialPortfolio.APIGateway.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/categories")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status500InternalServerError)]
    public class CategoriesController : ApiControllerBase
    {
        private readonly ICommandPublisher _commandPublisher;
        private readonly Category.CategoryClient _categoryClient;
        private readonly Order.OrderClient _orderClient;
        private readonly IMapper _mapper;

        public CategoriesController(
            ICommandPublisher commandPublisher, IUserInfoService userInfoService, 
            Category.CategoryClient categoryClient, Order.OrderClient orderClient, IMapper mapper) : base(userInfoService)
        {
            _commandPublisher = commandPublisher;
            _categoryClient = categoryClient;
            _orderClient = orderClient;
            _mapper = mapper;
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(WebApiResponse<CategoryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WebApiResponse<CategoryResponse>>> GetAsync()
        {
            var userId = await GetUserIdAsync();
            
            var query = new GetCategoryQuery { UserId = userId.ToString() };
            var response = await _categoryClient.GetAsync(query);
            
            var ordersQuery = _mapper.Map<GetOrdersQuery>((userId, "UserId"));
            var ordersResponse = await _orderClient.GetAllAsync(ordersQuery);
            
            var categoryResponse = _mapper.Map<CategoryResponse>((response, ordersResponse.Orders));
            
            return WebApiResponse.Success(categoryResponse);
        }
        
        [HttpPut]
        [ProducesResponseType(typeof(WebApiResponse), StatusCodes.Status202Accepted)]
        public async Task<ActionResult<WebApiResponse>> UpdateAsync([FromBody] UpdateCategoryRequest request)
        {
            var userId = await GetUserIdAsync();
            
            var updateCategoryCommand = new UpdateCategoryCommand(request.Name, request.Description, 
                request.ExpectedAllocationInPercentage, userId, request.SubCategories, request.Assets);
            await _commandPublisher.SendAsync(updateCategoryCommand);
            
            return WebApiResponse.Accepted();
        }
    }
}