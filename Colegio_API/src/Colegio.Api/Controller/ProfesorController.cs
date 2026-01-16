using Colegio.Application.Interfaces;
using Colegio.Application.Dtos;
using Microsoft.AspNetCore.Mvc;


namespace Colegio.Api.Controller;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProfesorController : ControllerBase
{
    private readonly IProfesorService _service;

    /// <summary>
    /// Controlador REST para gestionar profesores.
    /// </summary>
    /// <remarks>
    /// Regla importante:
    /// - No se puede eliminar un profesor si tiene notas asociadas (409).
    /// </remarks>
    public ProfesorController(IProfesorService service) => _service = service;

    /// <summary>
    /// Lista todos los profesores.
    /// </summary>
    /// <param name="ct">Token de cancelación.</param>
    /// <response code="200">Retorna la lista de profesores.</response>
    [HttpGet]
    [ProducesResponseType(typeof(ProfesorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(IReadOnlyList<ProfesorDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ProfesorDto>>> GetAll(CancellationToken ct)
        => Ok(await _service.GetAllAsync(ct));

    /// <summary>
    /// Obtiene un profesor por Id.
    /// </summary>
    /// <param name="id">Id del profesor.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <response code="200">Retorna el profesor.</response>
    /// <response code="404">Si el profesor no existe.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ProfesorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProfesorDto>> GetById(int id, CancellationToken ct)
        => Ok(await _service.GetByIdAsync(id, ct));

    /// <summary>
    /// Crea un profesor.
    /// </summary>
    /// <param name="dto">Datos del profesor.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <response code="201">Profesor creado.</response>
    /// <response code="400">DTO inválido (validación).</response>
    [HttpPost]
    [ProducesResponseType(typeof(ProfesorDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProfesorDto>> Create(CreateProfesorDto dto, CancellationToken ct)
    {
        var created = await _service.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>
    /// Actualiza un profesor.
    /// </summary>
    /// <param name="id">Id del profesor.</param>
    /// <param name="dto">Datos nuevos.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <response code="204">Actualización exitosa.</response>
    /// <response code="400">DTO inválido (validación).</response>
    /// <response code="404">Si el profesor no existe.</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, UpdateProfesorDto dto, CancellationToken ct)
    {
        await _service.UpdateAsync(id, dto, ct);
        return NoContent();
    }

    /// <summary>
    /// Elimina un profesor.
    /// </summary>
    /// <remarks>
    /// Regla: si el profesor tiene notas asociadas, debe responder 409 (Conflict).
    /// </remarks>
    /// <param name="id">Id del profesor.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <response code="204">Eliminación exitosa.</response>
    /// <response code="404">Si el profesor no existe.</response>
    /// <response code="409">Si el profesor tiene notas asociadas.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _service.DeleteAsync(id, ct);
        return NoContent();
    }
}