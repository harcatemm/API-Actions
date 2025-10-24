using MediatR;
using APP.DTO;

namespace APP.Features.Products.Queries
{
    public record GetProductByIdQuery(int Id) : IRequest<ProductDTO?>;
    
}
