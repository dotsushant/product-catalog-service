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
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateDto productCreateDto)
        {
            var commandResult = await _mediator.Send(new CreateProductCommand(productCreateDto));
            return commandResult ? Ok(commandResult.Result) : StatusCode(500, commandResult.FailureReason);
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                var commandResult = await _mediator.Send(new GetProductsCommand());
                return commandResult ? Ok(commandResult.Result) : NotFound(commandResult.FailureReason);
            }
            else
            {
                var commandResult = await _mediator.Send(new GetProductsByNameCommand(name));
                return commandResult ? Ok(commandResult.Result) : NotFound(commandResult.FailureReason);
            }
        }

        [HttpGet("{productId:guid}")]
        public async Task<IActionResult> GetProduct(Guid productId)
        {
            var commandResult = await _mediator.Send(new GetProductByIdCommand(productId));
            return commandResult ? Ok(commandResult.Result) : NotFound(commandResult.FailureReason);
        }

        [HttpPut("{productId}")]
        public async Task<IActionResult> UpdateProduct(Guid productId,
            [FromBody] ProductUpdateRequestDto productUpdateRequestDto)
        {
            var commandResult = await _mediator.Send(new UpdateProductCommand(productId, productUpdateRequestDto));
            return commandResult ? Ok(commandResult.Result) : StatusCode(500, commandResult.FailureReason);
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteProduct(Guid productId)
        {
            var commandResult = await _mediator.Send(new DeleteProductCommand(productId));
            return commandResult ? Ok(commandResult.Result) : StatusCode(500, commandResult.FailureReason);
        }
    }
}