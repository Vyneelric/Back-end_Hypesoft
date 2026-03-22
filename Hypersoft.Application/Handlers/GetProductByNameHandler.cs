using AutoMapper;
using Hypersoft.Application.Queries;
using Hypersoft.Domain.Repositories;
using MediatR;

namespace Hypersoft.Application.Handlers;

public class GetProductsByNameHandler : IRequestHandler<GetProductsByNameQuery, IEnumerable<ProductDto>>
{
    private readonly IProductRepository _repository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public GetProductsByNameHandler(IProductRepository repository, ICategoryRepository categoryRepository, IMapper mapper)
    {
        _repository = repository;
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<ProductDto>> Handle(GetProductsByNameQuery request, CancellationToken cancellationToken)
    {
        var products = await _repository.GetByNameAsync(request.name);

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
