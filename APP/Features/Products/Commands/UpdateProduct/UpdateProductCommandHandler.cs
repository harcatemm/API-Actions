using APP.Interfaces;
using Domain.Entities;
using MediatR;

namespace APP.Features.Products.Commands
{
    public class UpdateProductCommandHandler
    {
        private readonly IUnitOfWork _uow;
        public UpdateProductCommandHandler(IUnitOfWork uow)
        {
            this._uow = uow;
        }

        public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
                return false;

            var existingProduct = await _uow.Products.GetByIdAsync(request.Id);
            if (existingProduct is not null)
            {
                existingProduct.Name = request.Name;
                existingProduct.Price = request.Price;
                existingProduct.Description = request.Description;
                existingProduct.CategoryId = request.CategoryId;

                _uow.Products.Update(existingProduct);
                await _uow.CommitAsync();
                return true;
            }
            else
                return false;
        }
    }
}
