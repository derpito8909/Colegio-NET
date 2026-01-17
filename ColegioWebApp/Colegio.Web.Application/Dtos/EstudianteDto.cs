namespace Colegio.Web.Application.Dtos;

public record EstudianteDto(int Id, string Nombre);
public record CreateEstudianteDto(string Nombre);
public record UpdateEstudianteDto(string Nombre);