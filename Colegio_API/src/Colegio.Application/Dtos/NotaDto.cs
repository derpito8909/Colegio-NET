namespace Colegio.Application.Dtos;

public record NotaDto(int Id, string Nombre, int IdProfesor, int IdEstudiante, decimal Valor);

public record CreateNotaDto(string Nombre, int IdProfesor, int IdEstudiante, decimal Valor);

public record UpdateNotaDto(string Nombre, int IdProfesor, int IdEstudiante, decimal Valor);

public record NotaDetalleDto(
    int Id,
    string Nombre,
    decimal Valor,
    int IdProfesor,
    string ProfesorNombre,
    int IdEstudiante,
    string EstudianteNombre
);