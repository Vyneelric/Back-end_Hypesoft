using Hypersoft.Application.Queries;
using Hypersoft.Domain.Repositories;
using MediatR;

namespace Hypersoft.Application.Handlers;

public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductDto>>
{
    private readonly IProductRepository _repository;
    private readonly ICategoryRepository _categoryRepository;

    public GetAllProductsHandler(IProductRepository repository, ICategoryRepository categoryRepository)
    {
        _repository = repository;
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _repository.GetAllAsync();

        if (request.EstoqueMenorQue.HasValue)
            products = products.Where(p => p.quantidade_estoque < request.EstoqueMenorQue.Value);
        
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
