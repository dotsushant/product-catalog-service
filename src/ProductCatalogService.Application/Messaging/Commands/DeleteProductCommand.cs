using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ProductCatalogService.Application.Interfaces.Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProductCatalogService.Application.Messaging.Commands
{
    public class DeleteProductCommand : IRequest<CommandResult<bool>>
    {
        public DeleteProductCommand(Guid productId)
        {
            ProductId = productId;
        }

        public Guid ProductId { get; set; }
    }

    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, CommandResult<bool>>
    {
        private readonly ILogger<DeleteProductCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IProductWriteRepository _productWriteRepository;

        public DeleteProductCommandHandler(IMapper mapper, IProductWriteRepository productWriteRepository,
            ILogger<DeleteProductCommandHandler> logger)
        {
            _mapper = mapper;
            _productWriteRepository = productWriteRepository;
            _logger = logger;
        }

        public async Task<CommandResult<bool>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _productWriteRepository.DeleteProduct(request.ProductId);
                return new CommandResult<bool>(true);
            }
            catch (Exception e)
            {
                _logger.LogError(e, Resource.ProductCouldNotBeDeleted);
                return new CommandResult<bool>(Resource.ProductCouldNotBeDeleted);
            }
        }
    }
}