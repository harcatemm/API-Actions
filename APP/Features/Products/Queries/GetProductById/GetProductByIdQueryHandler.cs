using MediatR;
using APP.DTO;
using APP.Interfaces;

namespace APP.Features.Products.Queries
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDTO?>
    {
        private readonly IUnitOfWork _uow;
        public GetProductByIdQueryHandler(IUnitOfWork uow)
        {
            this._uow = uow;
        }

        public async Task<ProductDTO?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _uow.Products.GetByIdAsync(request.Id);
            if (product is not null)
            {
                return new ProductDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Description = product.Description,
                    CategoryId = product.CategoryId
                };
            }
            else
            {
                return null;
            }
        }
    }
}
