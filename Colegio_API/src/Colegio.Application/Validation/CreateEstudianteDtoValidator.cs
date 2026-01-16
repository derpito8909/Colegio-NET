using Colegio.Application.Dtos;
using FluentValidation;

namespace Colegio.Application.Validation;

public sealed class CreateEstudianteDtoValidator : AbstractValidator<CreateEstudianteDto>
{
    public CreateEstudianteDtoValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre del estudiante es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre del estudiante no puede superar 100 caracteres.");
    }
}