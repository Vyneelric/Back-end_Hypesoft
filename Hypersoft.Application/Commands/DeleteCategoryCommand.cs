using MediatR;

namespace Hypersoft.Application.Commands;

public record DeleteCategoryCommand(
    string id
) : IRequest<bool>;
