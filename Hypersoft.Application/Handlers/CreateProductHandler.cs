using FluentValidation;
using Hypersoft.Application.Commands;
using Hypersoft.Domain.Entities;
using Hypersoft.Domain.Repositories;
using MediatR;

namespace Hypersoft.Application.Handlers;

public class CreateProductHandler : IRequestHandler<CreateProductCommand, string>
{
    private readonly IProductRepository _repository;
    private readonly IValidator<CreateProductCommand> _validator;

    public CreateProductHandler(IProductRepository repository, IValidator<CreateProductCommand> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<string> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var product = new Product
        {
            nome = request.nome,
            descricao = request.descricao,
            preco = request.preco,
            quantidade_estoque = request.quantidade_estoque
        };

        var created = await _repository.CreateAsync(product);
        return created.id;
    }
}
