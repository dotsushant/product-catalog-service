using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ProductCatalogService.Application.DTO;
using ProductCatalogService.Application.Interfaces.Persistence;
using ProductCatalogService.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProductCatalogService.Application.Messaging.Commands
{
    public class CreateProductOptionCommand : IRequest<CommandResult<ProductOptionDto>>
    {
        public CreateProductOptionCommand(Guid productId, ProductOptionCreateDto productCreateDto)
        {
            ProductId = productId;
            ProductCreateDto = productCreateDto;
        }

        public Guid ProductId { get; }
        public ProductOptionCreateDto ProductCreateDto { get; }
    }

    public class
        CreateProductOptionCommandHandler : IRequestHandler<CreateProductOptionCommand, CommandResult<ProductOptionDto>>
    {
        private readonly ILogger<CreateProductOptionCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IProductWriteRepository _productWriteRepository;

        public CreateProductOptionCommandHandler(IMapper mapper, IProductWriteRepository productWriteRepository,
            ILogger<CreateProductOptionCommandHandler> logger)
        {
            _mapper = mapper;
            _productWriteRepository = productWriteRepository;
            _logger = logger;
        }

        public async Task<CommandResult<ProductOptionDto>> Handle(CreateProductOptionCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var productOption = new ProductOption(request.ProductId,
                    request.ProductCreateDto.Name, request.ProductCreateDto.Description);

                await _productWriteRepository.CreateProductOption(productOption);

                return new CommandResult<ProductOptionDto>(_mapper.Map<ProductOptionDto>(productOption));
            }
            catch (Exception e)
            {
                _logger.LogError(e, Resource.ProductOptionCouldNotBeCreated);
                return new CommandResult<ProductOptionDto>(Resource.ProductOptionCouldNotBeCreated);
            }
        }
    }
}