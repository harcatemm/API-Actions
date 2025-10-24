using MediatR;
using APP.DTO;

namespace APP.Features.Products.Queries
{
    public record GetProductWithCategoryQuery(int Id) : IRequest<ProductWithCategoryDTO?>;

}
