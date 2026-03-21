using MediatR;

namespace Hypersoft.Application.Queries;

public record GetProductByIdQuery(string Id) : IRequest<ProductDto?>;

public class ProductDto
{
    public string id { get; set; } = string.Empty;
    public string nome { get; set; } = string.Empty;
    public string descricao { get; set; } = string.Empty;
    public decimal preco { get; set; }
    public int quantidade_estoque { get; set; }
    public CategoryDto? category { get; set; }
}
