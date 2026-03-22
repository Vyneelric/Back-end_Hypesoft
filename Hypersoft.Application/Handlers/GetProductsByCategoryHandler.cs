using Hypersoft.Application.Queries;
using Hypersoft.Domain.Repositories;
using MediatR;

namespace Hypersoft.Application.Handlers;

public class GetProductsByCategoryHandler : IRequestHandler<GetProductsByCategoryQuery, IEnumerable<ProductDto>>
{
    private readonly IProductRepository _repository;
    private readonly ICategoryRepository _categoryRepository;

    public GetProductsByCategoryHandler(IProductRepository repository, ICategoryRepository categoryRepository)
    {
        _repository = repository;
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<ProductDto>> Handle(GetProductsByCategoryQuery request, CancellationToken cancellationToken)
    {
        var products = await _repository.GetByCategoryIdAsync(request.categoria_id);
        
        var productDtos = new List<ProductDto>();

        foreach (var product in products)
        {
            var category = await _categoryRepository.GetByIdAsync(product.categoria_id);

            productDtos.Add(new ProductDto
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
            });
        }

        return productDtos;
    }
}
