using MediatR;

namespace APP.Features.Products.Commands
{
    public record UpdateProductCommand(
        int Id,
        string Name,
        string Description,
        decimal Price,
        int CategoryId) : IRequest<bool>;    
}
