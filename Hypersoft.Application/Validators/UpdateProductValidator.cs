using FluentValidation;
using Hypersoft.Application.Commands;

namespace Hypersoft.Application.Validators;

public class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductValidator()
    {
        RuleFor(x => x.id).NotEmpty();
        RuleFor(x => x.nome).NotEmpty().MaximumLength(100).Must(v => v == v.Trim());
        RuleFor(x => x.descricao).NotEmpty().MaximumLength(500).Must(v => v == v.Trim());
        RuleFor(x => x.preco).GreaterThan(0);
        RuleFor(x => x.quantidade_estoque).GreaterThanOrEqualTo(0);
        RuleFor(x => x.categoria_id).NotEmpty();
    }
}
