using FluentValidation;
using Hypersoft.Application.Commands;
using Hypersoft.Domain.Entities;
using Hypersoft.Domain.Repositories;
using MediatR;

namespace Hypersoft.Application.Handlers;

public class CreateProductHandler : IRequestHandler<CreateProductCommand, string>
{
    private readonly IProductRepository _repository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IValidator<CreateProductCommand> _validator;

    public CreateProductHandler(IProductRepository repository, ICategoryRepository categoryRepository, IValidator<CreateProductCommand> validator)
    {
        _repository = repository;
        _categoryRepository = categoryRepository;
        _validator = validator;
    }

    public async Task<string> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var category = await _categoryRepository.GetByIdAsync(request.categoria_id);

        if(category == null)
        {
            throw new Exception("Categoria não encontrada");
        }

        var product = new Product
        {
            nome = request.nome,
            descricao = request.descricao,
            preco = request.preco,
            quantidade_estoque = request.quantidade_estoque,
            categoria_id = request.categoria_id
        };

        var created = await _repository.CreateAsync(product);
        return created.id;
    }
}
