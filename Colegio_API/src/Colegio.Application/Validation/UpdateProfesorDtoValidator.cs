using Colegio.Application.Dtos;
using FluentValidation;

namespace Colegio.Application.Validation;

public sealed class UpdateProfesorDtoValidator : AbstractValidator<UpdateProfesorDto>
{
    public UpdateProfesorDtoValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre del profesor es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre del profesor no puede superar 100 caracteres.");
    }
}