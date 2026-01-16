using Colegio.Application.Dtos;
using FluentValidation;

namespace Colegio.Application.Validation;

public sealed class UpdateNotaDtoValidator : AbstractValidator<UpdateNotaDto>
{
    public UpdateNotaDtoValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre de la nota es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre de la nota no puede superar 100 caracteres.");

        RuleFor(x => x.IdProfesor)
            .GreaterThan(0).WithMessage("IdProfesor debe ser un entero positivo.");

        RuleFor(x => x.IdEstudiante)
            .GreaterThan(0).WithMessage("IdEstudiante debe ser un entero positivo.");

        RuleFor(x => x.Valor)
            .InclusiveBetween(0, 10).WithMessage("El valor de la nota debe estar entre 0 y 10.");
    }
}