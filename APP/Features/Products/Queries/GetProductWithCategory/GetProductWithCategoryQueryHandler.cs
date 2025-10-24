using MediatR;
using APP.DTO;
using APP.Interfaces;

namespace APP.Features.Products.Queries.GetProductWithCategory
{
    public class GetProductWithCategoryQueryHandler : IRequestHandler<GetProductWithCategoryQuery, ProductWithCategoryDTO?>
    {
        private readonly IUnitOfWork _uow;

        public GetProductWithCategoryQueryHandler(IUnitOfWork uow)
        {
            this._uow = uow;
        }

        public async Task<ProductWithCategoryDTO?> Handle(GetProductWithCategoryQuery request, CancellationToken cancellationToken)
        {
            var product = await _uow.Products.GetByIdWithCategoryAsync(request.Id);

            if (product is not null)
            {
                return new ProductWithCategoryDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Description = product.Description,
                    Category  = new CategoryDTO
                    {
                        Id = product.Category.Id,
                        Name = product.Category.Name,
                        Description = product.Category.Description
                    }
                };
            }
            else return null;            
        }
    }
}
