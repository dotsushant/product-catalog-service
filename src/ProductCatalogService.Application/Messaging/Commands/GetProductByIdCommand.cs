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
    public class GetProductByIdCommand : IRequest<CommandResult<ProductDto>>
    {
        public GetProductByIdCommand(Guid productId)
        {
            ProductId = productId;
        }

        public Guid ProductId { get; }
    }

    public class GetProductByIdCommandHandler : IRequestHandler<GetProductByIdCommand, CommandResult<ProductDto>>
    {
        private readonly ILogger<GetProductByIdCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IProductReadRepository _productReadRepository;

        public GetProductByIdCommandHandler(IMapper mapper, IProductReadRepository productReadRepository,
            ILogger<GetProductByIdCommandHandler> logger)
        {
            _mapper = mapper;
            _productReadRepository = productReadRepository;
            _logger = logger;
        }

        public async Task<CommandResult<ProductDto>> Handle(GetProductByIdCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var products = await _productReadRepository.GetProductById(request.ProductId);
                return new CommandResult<ProductDto>(_mapper.Map<ProductDto>(products));
            }
            catch (Exception e)
            {
                _logger.LogError(e, Resource.ProductCouldNotBeRetrieved);
                return new CommandResult<ProductDto>(Resource.ProductCouldNotBeRetrieved);
            }
        }
    }
}