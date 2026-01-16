namespace Colegio.Application.Exceptions;

/// <summary>
/// Excepción que indica un conflicto con el estado actual del sistema.
/// Se usa para que el middleware devuelva HTTP 409 (Conflict).
/// </summary>
public sealed class ConflictException : Exception
{
    /// <summary>
    /// Crea una excepción de conflicto con un mensaje específico.
    /// </summary>
    /// <param name="message">Mensaje descriptivo del conflicto.</param>
    public ConflictException(string message) : base(message) { }
}