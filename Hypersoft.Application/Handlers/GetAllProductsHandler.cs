using AutoMapper;
using Hypersoft.Application.Queries;
using Hypersoft.Domain.Repositories;
using MediatR;

namespace Hypersoft.Application.Handlers;

public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductDto>>
{
    private readonly IProductRepository _repository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public GetAllProductsHandler(IProductRepository repository, ICategoryRepository categoryRepository, IMapper mapper)
    {
        _repository = repository;
        _categoryRepository = categoryRepository;
        _mapper = mapper;
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
            var productDto = _mapper.Map<ProductDto>(product);
            productDto.category = category != null ? _mapper.Map<CategoryDto>(category) : null;
            productDtos.Add(productDto);
        }

        return productDtos;
    }
}
