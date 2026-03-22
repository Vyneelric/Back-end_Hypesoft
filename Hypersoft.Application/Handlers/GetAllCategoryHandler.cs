using Hypersoft.Application.Queries;
using Hypersoft.Domain.Repositories;
using MediatR;

namespace Hypersoft.Application.Handlers;

public class GetAllCategoryHandler : IRequestHandler<GetAllCategoryQuery, IEnumerable<CategoryDto>>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetAllCategoryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<CategoryDto>> Handle(GetAllCategoryQuery request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetAllAsync();
        
        var categoryDtos = new List<CategoryDto>();

        foreach (var categoryI in category)
        {

            categoryDtos.Add(new CategoryDto
            {
                id = categoryI.id,
                nome = categoryI.nome,
                 descricao = categoryI.descricao
            });
        }

        return categoryDtos;
    }
}
