namespace Colegio.Application.Exceptions;

/// <summary>
/// Excepción que indica que un recurso solicitado no existe.
/// Se usa para que el middleware devuelva HTTP 404 (Not Found) con un mensaje claro.
/// </summary>
public sealed class NotFoundException : Exception
{
    /// <summary>
    /// Crea una excepción de recurso no encontrado con un mensaje específico.
    /// </summary>
    /// <param name="message">Mensaje descriptivo del error.</param>
    public NotFoundException(string message) : base(message) { }

    /// <summary>
    /// Crea una excepción estándar de "no encontrado" para una entidad e Id.
    /// </summary>
    /// <param name="entityName">Nombre lógico de la entidad (Ej: "Estudiante").</param>
    /// <param name="id">Identificador solicitado.</param>
    /// <returns>Una instancia de <see cref="NotFoundException"/> con mensaje estándar.</returns>
    public static NotFoundException For(string entityName, int id)
        => new($"{entityName} con Id={id} no existe.");
}