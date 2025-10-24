using MediatR;

namespace APP.Features.Products.Commands
{
    public record DeleteProductCommand(int Id) : IRequest<bool>;    
}
