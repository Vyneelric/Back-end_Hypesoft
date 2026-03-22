using MediatR;

namespace Hypersoft.Application.Queries;

public record GetProductsByNameQuery(string name) : IRequest<IEnumerable<ProductDto>>;
