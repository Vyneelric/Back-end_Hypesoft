using MediatR;

namespace Hypersoft.Application.Commands;

public record DeleteProductCommand(
    string id
) : IRequest<bool>;
