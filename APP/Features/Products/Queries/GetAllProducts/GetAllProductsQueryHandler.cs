using MediatR;
using APP.DTO;
using APP.Interfaces;

namespace APP.Features.Products.Queries
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductDTO?>>
    {
        private readonly IUnitOfWork _uow;

        public GetAllProductsQueryHandler(IUnitOfWork uow)
        {
            this._uow = uow;
        }

        public async Task<IEnumerable<ProductDTO?>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _uow.Products.GetAllAsync();
            if (products.Any())
                return products.Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Description = p.Description,
                    CategoryId = p.CategoryId
                });
            else
                return Enumerable.Empty<ProductDTO>();
        }
    }
}
