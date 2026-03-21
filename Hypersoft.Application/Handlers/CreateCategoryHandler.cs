using FluentValidation;
using Hypersoft.Application.Commands;
using Hypersoft.Domain.Entities;
using Hypersoft.Domain.Repositories;
using MediatR;

namespace Hypersoft.Application.Handlers;

public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, string>
{
    private readonly ICategoryRepository _repository;
    private readonly IValidator<CreateCategoryCommand> _validator;

    public CreateCategoryHandler(ICategoryRepository repository, IValidator<CreateCategoryCommand> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<string> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var category = new Category
        {
            nome = request.nome,
            descricao = request.descricao
        };

        var created = await _repository.CreateAsync(category);
        return created.id;
    }
}
