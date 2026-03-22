using FluentValidation;
using Hypersoft.Application.Commands;
using Hypersoft.Domain.Entities;
using Hypersoft.Domain.Repositories;
using MediatR;

namespace Hypersoft.Application.Handlers;

public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, bool>
{
    private readonly IProductRepository _repository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IValidator<UpdateProductCommand> _validator;

    public UpdateProductHandler(IProductRepository repository, ICategoryRepository categoryRepository, IValidator<UpdateProductCommand> validator)
    {
        _repository = repository;
        _categoryRepository = categoryRepository;
        _validator = validator;
    }

    public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
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
            id = request.id,
            nome = request.nome,
            descricao = request.descricao,
            preco = request.preco,
            quantidade_estoque = request.quantidade_estoque,
            categoria_id = request.categoria_id
        };

        var result = await _repository.UpdateAsync(request.id, product);
        return result;
    }
}
