using MediatR;

namespace Hypersoft.Application.Commands;

public record CreateCategoryCommand(
    string nome,
    string descricao
) : IRequest<string>;
