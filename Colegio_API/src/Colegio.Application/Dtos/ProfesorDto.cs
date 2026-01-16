namespace Colegio.Application.Dtos;

public record ProfesorDto(int Id, string Nombre);

public record CreateProfesorDto(string Nombre);

public record UpdateProfesorDto(string Nombre);