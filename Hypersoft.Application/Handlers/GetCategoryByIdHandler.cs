using Hypersoft.Application.Queries;
using Hypersoft.Domain.Repositories;
using MediatR;

namespace Hypersoft.Application.Handlers;

public class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDto?>
{
    private readonly ICategoryRepository _repository;

    public GetCategoryByIdHandler(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<CategoryDto?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _repository.GetByIdAsync(request.Id);
        
        if (category == null) 
            return null;

        return new CategoryDto
        {
            id = category.id,
            nome = category.nome,
            descricao = category.descricao
        };
    }
}
