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
    public class GetProductsByNameCommand : IRequest<CommandResult<ProductDtoCollection>>
    {
        public GetProductsByNameCommand(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }

    public class
        GetProductsByNameCommandHandler : IRequestHandler<GetProductsByNameCommand, CommandResult<ProductDtoCollection>>
    {
        private readonly ILogger<GetProductsByNameCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IProductReadRepository _productReadRepository;

        public GetProductsByNameCommandHandler(IMapper mapper, IProductReadRepository productReadRepository,
            ILogger<GetProductsByNameCommandHandler> logger)
        {
            _mapper = mapper;
            _productReadRepository = productReadRepository;
            _logger = logger;
        }

        public async Task<CommandResult<ProductDtoCollection>> Handle(GetProductsByNameCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var products = await _productReadRepository.GetProductsByName(request.Name);
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