using Hypersoft.Application.Queries;
using Hypersoft.Domain.Repositories;
using MediatR;

namespace Hypersoft.Application.Handlers;

public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
{
    private readonly IProductRepository _repository;
    private readonly ICategoryRepository _categoryRepository;

    public GetProductByIdHandler(IProductRepository repository, ICategoryRepository categoryRepository)
    {
        _repository = repository;
        _categoryRepository = categoryRepository;
    }

    public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetByIdAsync(request.Id);
        
        if (product == null) 
            return null;

        var category = await _categoryRepository.GetByIdAsync(product.categoria_id);

        return new ProductDto
        {
            id = product.id,
            nome = product.nome,
            descricao = product.descricao,
            preco = product.preco,
            quantidade_estoque = product.quantidade_estoque,
            category = category != null ? new CategoryDto
            {
                id = category.id,
                nome = category.nome,
                descricao = category.descricao
            } : null
        };
    }
}
