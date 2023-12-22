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
    public class UpdateProductOptionCommand : IRequest<CommandResult<ProductOptionDto>>
    {
        public UpdateProductOptionCommand(Guid productId, Guid productOptionId,
            ProductOptionUpdateRequestDto productOptionUpdateRequestDto)
        {
            ProductId = productId;
            ProductOptionId = productOptionId;
            ProductOptionUpdateRequestDto = productOptionUpdateRequestDto;
        }

        public Guid ProductId { get; }
        public Guid ProductOptionId { get; }
        public ProductOptionUpdateRequestDto ProductOptionUpdateRequestDto { get; set; }
    }

    public class
        UpdateProductOptionCommandHandler : IRequestHandler<UpdateProductOptionCommand, CommandResult<ProductOptionDto>>
    {
        private readonly ILogger<UpdateProductOptionCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IProductWriteRepository _productWriteRepository;

        public UpdateProductOptionCommandHandler(IMapper mapper, IProductWriteRepository productWriteRepository,
            ILogger<UpdateProductOptionCommandHandler> logger)
        {
            _mapper = mapper;
            _productWriteRepository = productWriteRepository;
            _logger = logger;
        }

        public async Task<CommandResult<ProductOptionDto>> Handle(UpdateProductOptionCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                await _productWriteRepository.UpdateProductOption(request.ProductOptionId,
                    request.ProductOptionUpdateRequestDto.Name, request.ProductOptionUpdateRequestDto.Description);

                var productOptionDto = _mapper.Map<ProductOptionDto>(request.ProductOptionUpdateRequestDto);
                productOptionDto.Id = request.ProductOptionId;
                productOptionDto.ProductId = request.ProductId;
                return new CommandResult<ProductOptionDto>(productOptionDto);
            }
            catch (Exception e)
            {
                _logger.LogError(e, Resource.ProductOptionCouldNotBeUpdated);
                return new CommandResult<ProductOptionDto>(Resource.ProductOptionCouldNotBeUpdated);
            }
        }
    }
}