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
    public class CreateProductCommand : IRequest<CommandResult<ProductDto>>
    {
        public CreateProductCommand(ProductCreateDto productCreateDto)
        {
            ProductCreateDto = productCreateDto;
        }

        public ProductCreateDto ProductCreateDto { get; set; }
    }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, CommandResult<ProductDto>>
    {
        private readonly ILogger<CreateProductCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IProductWriteRepository _productWriteRepository;

        public CreateProductCommandHandler(IMapper mapper, IProductWriteRepository productWriteRepository,
            ILogger<CreateProductCommandHandler> logger)
        {
            _mapper = mapper;
            _productWriteRepository = productWriteRepository;
            _logger = logger;
        }

        public async Task<CommandResult<ProductDto>> Handle(CreateProductCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var product = new Product(request.ProductCreateDto.Name,
                    request.ProductCreateDto.Description, request.ProductCreateDto.Price,
                    request.ProductCreateDto.DeliveryPrice);
                await _productWriteRepository.CreateProduct(product);
                return new CommandResult<ProductDto>(_mapper.Map<ProductDto>(product));
            }
            catch (Exception e)
            {
                _logger.LogError(e, Resource.ProductCouldNotBeCreated);
                return new CommandResult<ProductDto>(Resource.ProductCouldNotBeCreated);
            }
        }
    }
}