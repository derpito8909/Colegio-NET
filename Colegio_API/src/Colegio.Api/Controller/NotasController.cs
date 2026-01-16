using Colegio.Application.Dtos;
using Colegio.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Colegio.Api.Controller;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class NotasController : ControllerBase
{
    private readonly INotaService _service;

    /// <summary>
    /// Controlador REST para gestionar notas.
    /// </summary>
    /// <remarks>
    /// Notas soporta dos vistas:
    /// - <b>Simple</b>: datos básicos de la nota.
    /// - <b>Detalle</b>: incluye nombres de profesor y estudiante.
    /// </remarks>
    public NotasController(INotaService service) => _service = service;

    /// <summary>
    /// Lista todas las notas (formato simple).
    /// </summary>
    /// <remarks>
    /// Útil para listados rápidos. No incluye los nombres de profesor/estudiante.
    /// Para eso usa <c>GET /api/notas/detalle</c>.
    /// </remarks>
    /// <response code="200">Retorna la lista de notas.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<NotaDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<NotaDto>>> GetAll(CancellationToken ct)
        => Ok(await _service.GetAllAsync(ct));

    /// <summary>
    /// Lista todas las notas en formato detalle (incluye nombres relacionados).
    /// </summary>
    /// <response code="200">Retorna la lista de notas con detalle.</response>
    [HttpGet("detalle")]
    [ProducesResponseType(typeof(IReadOnlyList<NotaDetalleDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<NotaDetalleDto>>> GetAllDetalle(CancellationToken ct)
        => Ok(await _service.GetAllDetailAsync(ct));

    /// <summary>
    /// Obtiene una nota por Id (formato simple).
    /// </summary>
    /// <param name="id">Id de la nota.</param>
    /// <response code="200">Retorna la nota.</response>
    /// <response code="404">Si la nota no existe.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(NotaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<NotaDto>> GetById(int id, CancellationToken ct)
    {
        var nota = await _service.GetByIdAsync(id, ct);
        return Ok(nota);
    }

    /// <summary>
    /// Obtiene una nota por Id (vista detalle).
    /// </summary>
    /// <remarks>
    /// Devuelve información ampliada, incluyendo nombres de profesor y estudiante.
    /// </remarks>
    /// <param name="id">Id de la nota.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <response code="200">Retorna la nota en vista detalle.</response>
    /// <response code="404">Si la nota no existe.</response>
    [HttpGet("{id:int}/detalle")]
    [ProducesResponseType(typeof(NotaDetalleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<NotaDetalleDto>> GetByIdDetalle(int id, CancellationToken ct)
    {
        var nota = await _service.GetByIdDetalleAsync(id, ct);
        return Ok(nota);
    }

    /// <summary>
    /// Crea una nueva nota.
    /// </summary>
    /// <param name="dto">Datos de la nota a crear.</param>
    /// <response code="201">Nota creada correctamente.</response>
    /// <response code="400">DTO inválido (validación).</response>
    /// <response code="404">Si el profesor o el estudiante no existen.</response>
    [HttpPost]
    [ProducesResponseType(typeof(NotaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<NotaDto>> Create(CreateNotaDto dto, CancellationToken ct)
    {
        var created = await _service.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>
    /// Actualiza una nota existente.
    /// </summary>
    /// <remarks>
    /// Reglas:
    /// - La nota debe existir.
    /// - El profesor y estudiante asociados deben existir.
    /// </remarks>
    /// <param name="id">Id de la nota a actualizar.</param>
    /// <param name="dto">Datos nuevos de la nota.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <response code="204">Actualización exitosa (sin contenido).</response>
    /// <response code="400">DTO inválido (validación).</response>
    /// <response code="404">Si la nota (o profesor/estudiante) no existe.</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, UpdateNotaDto dto, CancellationToken ct)
    {
        await _service.UpdateAsync(id, dto, ct);
        return NoContent();
    }

    /// <summary>
    /// Elimina una nota por Id.
    /// </summary>
    /// <param name="id">Id de la nota a eliminar.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <response code="204">Eliminación exitosa (sin contenido).</response>
    /// <response code="404">Si la nota no existe.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
         await _service.DeleteAsync(id, ct);
        return NoContent();
    }
}