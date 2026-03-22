using MediatR;

namespace Hypersoft.Application.Queries;

public record GetAllProductsQuery() : IRequest<IEnumerable<ProductDto>>{
    public int? EstoqueMenorQue { get; set; }
}


