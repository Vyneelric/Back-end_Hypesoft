using MediatR;

namespace Hypersoft.Application.Queries;

public record GetProductsByCategoryQuery(string categoria_id) : IRequest<IEnumerable<ProductDto>>;
