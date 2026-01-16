using Colegio.Application.Dtos;

namespace Colegio.Application.Interfaces;

/// <summary>
/// Servicio de notas.
/// Contiene reglas de negocio y operaciones de consulta/creación/edición/eliminación.
/// </summary>
/// <remarks>
/// Reglas relevantes:
/// - Para crear o actualizar una nota, el profesor y estudiante deben existir.
/// - Cuando se solicita "detalle", la respuesta incluye nombres de profesor y estudiante.
/// </remarks>
public interface INotaService
{
    /// <summary>
    /// Obtiene todas las notas en formato simple (sin nombres relacionados).
    /// </summary>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Listado de notas.</returns>
    Task<IReadOnlyList<NotaDto>> GetAllAsync(CancellationToken ct);
    /// <summary>
    /// Obtiene todas las notas en formato detalle (incluye nombres de profesor y estudiante).
    /// </summary>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Listado de notas con información ampliada.</returns>
    Task<IReadOnlyList<NotaDetalleDto>> GetAllDetailAsync(CancellationToken ct);

    /// <summary>
    /// Obtiene una nota por Id en formato simple.
    /// </summary>
    /// <param name="id">Id de la nota.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>La nota si existe; si no existe, se devuelve null.</returns>
    Task<NotaDto?> GetByIdAsync(int id, CancellationToken ct);
    
    /// <summary>
    /// Obtiene una nota por Id en formato detalle.
    /// </summary>
    /// <param name="id">Id de la nota.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>La nota detalle si existe; si no existe, se devuelve null.</returns>
    Task<NotaDetalleDto?> GetByIdDetalleAsync(int id, CancellationToken ct);

    /// <summary>
    /// Crea una nota.
    /// </summary>
    /// <param name="dto">Datos necesarios para crear la nota.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Nota creada en formato simple.</returns>
    /// <exception cref="Colegio.Application.Exceptions.NotFoundException">
    /// Se lanza si el profesor o el estudiante no existen.
    /// </exception>
    Task<NotaDto> CreateAsync(CreateNotaDto dto, CancellationToken ct);
    
    /// <summary>
    /// Actualiza una nota existente.
    /// </summary>
    /// <param name="id">Id de la nota a actualizar.</param>
    /// <param name="dto">Datos nuevos.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>True si se actualizó; si no existe, lanza NotFoundException.</returns>
    /// <exception cref="Colegio.Application.Exceptions.NotFoundException">
    /// Se lanza si la nota, el profesor o el estudiante no existen.
    /// </exception>
    Task<bool> UpdateAsync(int id, UpdateNotaDto dto, CancellationToken ct);
    
    /// <summary>
    /// Elimina una nota por Id.
    /// </summary>
    /// <param name="id">Id de la nota.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>True si se eliminó; si no existe, lanza NotFoundException.</returns>
    /// <exception cref="Colegio.Application.Exceptions.NotFoundException">
    /// Se lanza si la nota no existe.
    /// </exception>
    Task<bool> DeleteAsync(int id, CancellationToken ct);
}