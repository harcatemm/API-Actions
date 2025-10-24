using MediatR;
using APP.DTO;

namespace APP.Features.Products.Queries
{
    public record GetAllProductsQuery() : IRequest<IEnumerable<ProductDTO?>>;    
}
