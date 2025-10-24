using APP.Interfaces;
using Domain.Entities;
using MediatR;


namespace APP.Features.Products.Commands
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, bool>
    {
        private readonly IUnitOfWork _uow;

        public CreateProductCommandHandler(IUnitOfWork uow)
        {
            this._uow = uow;
        }

        public async Task<bool> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _uow.Products.FindAsync(p => p.Name.ToLower().Equals(request.Name.ToLower()));
            if (!product.Any())
            {
                var newProduct = new Product
                {
                    Name = request.Name,
                    Price = request.Price,
                    Description = request.Description,
                    CategoryId = request.CategoryId
                };
                await _uow.Products.AddAsync(newProduct);
                await _uow.CommitAsync();
                return true;
            }
            else
                return false;
        }
    }
}
