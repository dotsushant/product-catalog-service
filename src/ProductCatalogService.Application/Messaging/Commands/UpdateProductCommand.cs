using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ProductCatalogService.Application.DTO;
using ProductCatalogService.Application.Interfaces.Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProductCatalogService.Application.Messaging.Commands
{
    public class UpdateProductCommand : IRequest<CommandResult<ProductDto>>
    {
        public UpdateProductCommand(Guid productId, ProductUpdateRequestDto productUpdateRequestDto)
        {
            ProductId = productId;
            ProductUpdateRequestDto = productUpdateRequestDto;
        }

        public Guid ProductId { get; set; }
        public ProductUpdateRequestDto ProductUpdateRequestDto { get; set; }
    }

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, CommandResult<ProductDto>>
    {
        private readonly ILogger<UpdateProductCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IProductWriteRepository _productWriteRepository;

        public UpdateProductCommandHandler(IMapper mapper, IProductWriteRepository productWriteRepository,
            ILogger<UpdateProductCommandHandler> logger)
        {
            _mapper = mapper;
            _productWriteRepository = productWriteRepository;
            _logger = logger;
        }

        public async Task<CommandResult<ProductDto>> Handle(UpdateProductCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                await _productWriteRepository.UpdateProduct(request.ProductId, request.ProductUpdateRequestDto.Name,
                    request.ProductUpdateRequestDto.Description, request.ProductUpdateRequestDto.Price,
                    request.ProductUpdateRequestDto.DeliveryPrice);

                var productDto = _mapper.Map<ProductDto>(request.ProductUpdateRequestDto);
                productDto.Id = request.ProductId;
                return new CommandResult<ProductDto>(productDto);
            }
            catch (Exception e)
            {
                _logger.LogError(e, Resource.ProductCouldNotBeUpdated);
                return new CommandResult<ProductDto>(Resource.ProductCouldNotBeUpdated);
            }
        }
    }
}