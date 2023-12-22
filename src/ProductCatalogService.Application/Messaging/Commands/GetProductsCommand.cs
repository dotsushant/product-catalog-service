using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ProductCatalogService.Application.DTO;
using ProductCatalogService.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ProductCatalogService.Application.Messaging.Commands
{
    public class GetProductsCommand : IRequest<CommandResult<ProductDtoCollection>>
    {
    }

    public class GetProductsCommandHandler : IRequestHandler<GetProductsCommand, CommandResult<ProductDtoCollection>>
    {
        private readonly ILogger<GetProductsCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IProductReadRepository _productReadRepository;

        public GetProductsCommandHandler(IMapper mapper, IProductReadRepository productReadRepository,
            ILogger<GetProductsCommandHandler> logger)
        {
            _mapper = mapper;
            _productReadRepository = productReadRepository;
            _logger = logger;
        }

        public async Task<CommandResult<ProductDtoCollection>> Handle(GetProductsCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var products = await _productReadRepository.GetProducts();
                var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);
                return new CommandResult<ProductDtoCollection>(new ProductDtoCollection(productDtos));
            }
            catch (Exception e)
            {
                _logger.LogError(e, Resource.ProductCouldNotBeRetrieved);
                return new CommandResult<ProductDtoCollection>(Resource.ProductCouldNotBeRetrieved);
            }
        }
    }
}