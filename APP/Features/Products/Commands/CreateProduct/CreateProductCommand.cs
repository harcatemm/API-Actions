using MediatR;

namespace APP.Features.Products.Commands
{
    public record CreateProductCommand(
        string Name,
        string Description,
        decimal Price,
        int CategoryId) : IRequest<bool>;

}
