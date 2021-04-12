using System.Threading.Tasks;
using FinancialPortfolio.APIGateway.Web.Messages;
using FinancialPortfolio.Messaging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinancialPortfolio.APIGateway.Web.Controllers
{
    [Route("api/transfers")]
    [ApiController]
    [Produces("application/json")]
    public class TransfersController : ControllerBase
    {
        private readonly IMessagePublisher _messagePublisher;

        public TransfersController(IMessagePublisher messagePublisher)
        {
            _messagePublisher = messagePublisher;
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Create()
        {
            var transferCreated = new CreateTransfer() {Name = "test"};
            await _messagePublisher.PublishAsync(transferCreated);
            return NoContent();
        }
    }
}