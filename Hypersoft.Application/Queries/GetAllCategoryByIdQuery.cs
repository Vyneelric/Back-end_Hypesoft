using MediatR;

namespace Hypersoft.Application.Queries;

public record GetAllCategoryQuery() : IRequest<IEnumerable<CategoryDto>>;
