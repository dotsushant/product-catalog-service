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
    public class GetProductOptionByIdCommand : IRequest<CommandResult<ProductOptionDto>>
    {
        public GetProductOptionByIdCommand(Guid productOptionId)
        {
            ProductOptionId = productOptionId;
        }

        public Guid ProductOptionId { get; }
    }

    public class GetProductOptionByIdCommandHandler : IRequestHandler<GetProductOptionByIdCommand,
        CommandResult<ProductOptionDto>>
    {
        private readonly ILogger<GetProductOptionByIdCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IProductReadRepository _productReadRepository;

        public GetProductOptionByIdCommandHandler(IMapper mapper, IProductReadRepository productReadRepository,
            ILogger<GetProductOptionByIdCommandHandler> logger)
        {
            _mapper = mapper;
            _productReadRepository = productReadRepository;
            _logger = logger;
        }

        public async Task<CommandResult<ProductOptionDto>> Handle(GetProductOptionByIdCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var productOptions = await _productReadRepository.GetProductOptionById(request.ProductOptionId);
                return new CommandResult<ProductOptionDto>(_mapper.Map<ProductOptionDto>(productOptions));
            }
            catch (Exception e)
            {
                _logger.LogError(e, Resource.ProductOptionCouldNotBeRetrieved);
                return new CommandResult<ProductOptionDto>(Resource.ProductOptionCouldNotBeRetrieved);
            }
        }
    }
}