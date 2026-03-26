using FluentValidation;
using Hypersoft.Application.Commands;

namespace Hypersoft.Application.Validators;

public class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryValidator()
    {
        RuleFor(x => x.nome).NotEmpty().MaximumLength(50).NotNull().Must(v => v == v.Trim());
        RuleFor(x => x.descricao).NotEmpty().MaximumLength(100).Must(v => v == v.Trim());
    }
}
