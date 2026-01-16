using Colegio.Application.Dtos;
using FluentValidation;

namespace Colegio.Application.Validation;

public sealed class CreateProfesorDtoValidator : AbstractValidator<CreateProfesorDto>
{
    public CreateProfesorDtoValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre del profesor es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre del profesor no puede superar 100 caracteres.");
    }
}