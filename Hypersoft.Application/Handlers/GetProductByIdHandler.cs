using Hypersoft.Application.Queries;
using Hypersoft.Domain.Repositories;
using MediatR;

namespace Hypersoft.Application.Handlers;

public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
{
    private readonly IProductRepository _repository;

    public GetProductByIdHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetByIdAsync(request.Id);
        
        if (product == null) 
            return null;

        return new ProductDto
        {
            id = product.id,
            nome = product.nome,
            descricao = product.descricao,
            preco = product.preco,
            quantidade_estoque = product.quantidade_estoque
        };
    }
}
