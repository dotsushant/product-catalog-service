using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ProductCatalogService.Application.Interfaces.Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProductCatalogService.Application.Messaging.Commands
{
    public class DeleteProductOptionCommand : IRequest<CommandResult<bool>>
    {
        public DeleteProductOptionCommand(Guid productOptionId)
        {
            ProductOptionId = productOptionId;
        }

        public Guid ProductOptionId { get; }
    }

    public class DeleteProductOptionCommandHandler : IRequestHandler<DeleteProductOptionCommand, CommandResult<bool>>
    {
        private readonly ILogger<DeleteProductOptionCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IProductWriteRepository _productWriteRepository;

        public DeleteProductOptionCommandHandler(IMapper mapper, IProductWriteRepository productWriteRepository,
            ILogger<DeleteProductOptionCommandHandler> logger)
        {
            _mapper = mapper;
            _productWriteRepository = productWriteRepository;
            _logger = logger;
        }

        public async Task<CommandResult<bool>> Handle(DeleteProductOptionCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                await _productWriteRepository.DeleteProductOption(request.ProductOptionId);
                return new CommandResult<bool>(true);
            }
            catch (Exception e)
            {
                _logger.LogError(e, Resource.ProductOptionCouldNotBeDeleted);
                return new CommandResult<bool>(Resource.ProductOptionCouldNotBeDeleted);
            }
        }
    }
}