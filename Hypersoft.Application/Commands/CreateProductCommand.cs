using MediatR;

namespace Hypersoft.Application.Commands;

public record CreateProductCommand(
    string nome,
    string descricao,
    decimal preco,
    int quantidade_estoque,
    string categoria_id
) : IRequest<string>;
