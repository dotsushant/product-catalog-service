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
    public class GetProductOptionsCommand : IRequest<CommandResult<ProductOptionDtoCollection>>
    {
        public GetProductOptionsCommand(Guid productId)
        {
            ProductId = productId;
        }

        public Guid ProductId { get; }
    }

    public class
        GetProductOptionsCommandHandler : IRequestHandler<GetProductOptionsCommand,
            CommandResult<ProductOptionDtoCollection>>
    {
        private readonly ILogger<GetProductOptionsCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IProductReadRepository _productReadRepository;

        public GetProductOptionsCommandHandler(IMapper mapper, IProductReadRepository productReadRepository,
            ILogger<GetProductOptionsCommandHandler> logger)
        {
            _mapper = mapper;
            _productReadRepository = productReadRepository;
            _logger = logger;
        }

        public async Task<CommandResult<ProductOptionDtoCollection>> Handle(GetProductOptionsCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var productOptions = await _productReadRepository.GetProductOptions(request.ProductId);
                var productOptionDtos = _mapper.Map<IEnumerable<ProductOptionDto>>(productOptions);
                return new CommandResult<ProductOptionDtoCollection>(new ProductOptionDtoCollection(productOptionDtos));
            }
            catch (Exception e)
            {
                _logger.LogError(e, Resource.ProductOptionCouldNotBeRetrieved);
                return new CommandResult<ProductOptionDtoCollection>(Resource.ProductOptionCouldNotBeRetrieved);
            }
        }
    }
}