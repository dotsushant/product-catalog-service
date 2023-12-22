using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductCatalogService.Application.DTO;
using ProductCatalogService.Application.Messaging.Commands;
using System;
using System.Threading.Tasks;

namespace ProductCatalogService.Web.Controllers
{
    [Route("products")]
    [ApiController]
    public class ProductOptionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductOptionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("{productId}/options")]
        public async Task<IActionResult> CreateProductOption(Guid productId,
            [FromBody] ProductOptionCreateDto productOptionCreateDto)
        {
            var commandResult = await _mediator.Send(new CreateProductOptionCommand(productId, productOptionCreateDto));
            return commandResult ? Ok(commandResult.Result) : StatusCode(500, commandResult.FailureReason);
        }

        [HttpGet("{productId}/options")]
        public async Task<IActionResult> GetProductOptions(Guid productId)
        {
            var commandResult = await _mediator.Send(new GetProductOptionsCommand(productId));
            return commandResult ? Ok(commandResult.Result) : NotFound(commandResult.FailureReason);
        }

        [HttpGet("{productId}/options/{productOptionId}")]
        public async Task<IActionResult> GetProductOption(Guid productId, Guid productOptionId)
        {
            var commandResult = await _mediator.Send(new GetProductOptionByIdCommand(productOptionId));
            return commandResult ? Ok(commandResult.Result) : NotFound(commandResult.FailureReason);
        }

        [HttpPut("{productId}/options/{productOptionId}")]
        public async Task<IActionResult> UpdateProductOption(Guid productId, Guid productOptionId,
            [FromBody] ProductOptionUpdateRequestDto productOptionUpdateDto)
        {
            var commandResult =
                await _mediator.Send(new UpdateProductOptionCommand(productId, productOptionId,
                    productOptionUpdateDto));
            return commandResult ? Ok(commandResult.Result) : StatusCode(500, commandResult.FailureReason);
        }

        [HttpDelete("{productId}/options/{productOptionId}")]
        public async Task<IActionResult> DeleteProductOption(Guid productId, Guid productOptionId)
        {
            var commandResult = await _mediator.Send(new DeleteProductOptionCommand(productId));
            return commandResult ? Ok(commandResult.Result) : StatusCode(500, commandResult.FailureReason);
        }
    }
}