using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using DividendApi;
using FinancialPortfolio.APIGateway.Contracts.Dividends.Requests;
using FinancialPortfolio.APIGateway.Web.Interfaces;
using FinancialPortfolio.Infrastructure.WebApi.Models.Response;
using FinancialPortfolio.ProblemDetails.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinancialPortfolio.APIGateway.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/accounts/{accountId:guid}/dividends")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(WebApiProblemDetails), StatusCodes.Status500InternalServerError)]
    public class DividendsController : ApiControllerBase
    {
        private readonly Dividend.DividendClient _dividendClient;

        public DividendsController(IUserInfoService userInfoService, Dividend.DividendClient dividendClient) : base(userInfoService)
        {
            _dividendClient = dividendClient;
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(WebApiResponse<IEnumerable<AccountDividendResponse>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<WebApiResponse<IEnumerable<AccountDividendResponse>>>> GetAccountDividendsAsync(
            [FromRoute] Guid accountId, [FromQuery] GetAccountDividendsRequest request)
        {
            var query = new GetAccountDividendsQuery
            {
                AccountIds = { new [] { accountId.ToString() } },
                StartDateTime = request.StartDateTime.ToString(CultureInfo.InvariantCulture),
                EndDateTime = request.EndDateTime.ToString(CultureInfo.InvariantCulture),
                AssetId = request.AssetId.ToString()
            };
            var response = await _dividendClient.GetAccountDividendsAsync(query);

            return WebApiResponse.Success(response.Records);
        }
    }
}