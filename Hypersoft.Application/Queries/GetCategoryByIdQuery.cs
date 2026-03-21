using MediatR;

namespace Hypersoft.Application.Queries;

public record GetCategoryByIdQuery(string Id) : IRequest<CategoryDto?>;

public class CategoryDto
{
    public string id { get; set; } = string.Empty;
    public string nome { get; set; } = string.Empty;
    public string descricao { get; set; } = string.Empty;
}
