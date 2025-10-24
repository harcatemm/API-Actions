using APP.Interfaces;
using Domain.Entities;
using MediatR;

namespace APP.Features.Products.Commands
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly IUnitOfWork _uow;
        public DeleteProductCommandHandler(IUnitOfWork uow)
        {
            this._uow = uow;
        }
        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _uow.Products.GetByIdAsync(request.Id);
            if (product is not null)
            {
                _uow.Products.Delete(product);
                await _uow.CommitAsync();
                return true;
            }
            else 
                return false;
        }
    }    
}
