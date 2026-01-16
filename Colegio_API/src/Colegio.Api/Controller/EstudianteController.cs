using Colegio.Application.Interfaces;
using Colegio.Application.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Colegio.Api.Controller;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class EstudianteController : ControllerBase
{
    private readonly IEstudianteService _service;

    /// <summary>
    /// Controlador REST para gestionar estudiantes.
    /// </summary>
    /// <remarks>
    /// Regla importante:
    /// - No se puede eliminar un estudiante si tiene notas asociadas (debe devolver 409).
    /// </remarks>
    public EstudianteController(IEstudianteService service) => _service = service;

    /// <summary>
    /// Lista todos los estudiantes.
    /// </summary>
    /// <param name="ct">Token de cancelación.</param>
    /// <response code="200">Retorna la lista de estudiantes.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<EstudianteDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<EstudianteDto>>> GetAll(CancellationToken ct)
        => Ok(await _service.GetAllAsync(ct));

    /// <summary>
    /// Obtiene un estudiante por Id.
    /// </summary>
    /// <param name="id">Id del estudiante.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <response code="200">Retorna el estudiante.</response>
    /// <response code="404">Si el estudiante no existe.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(EstudianteDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EstudianteDto>> GetById(int id, CancellationToken ct)
        => Ok(await _service.GetByIdAsync(id, ct));

    /// <summary>
    /// Crea un estudiante.
    /// </summary>
    /// <param name="dto">Datos del estudiante.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <response code="201">Estudiante creado.</response>
    /// <response code="400">DTO inválido (validación).</response>
    [HttpPost]
    [ProducesResponseType(typeof(EstudianteDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EstudianteDto>> Create(CreateEstudianteDto dto, CancellationToken ct)
    {
        var created = await _service.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>
    /// Actualiza un estudiante.
    /// </summary>
    /// <param name="id">Id del estudiante.</param>
    /// <param name="dto">Datos nuevos.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <response code="204">Actualización exitosa.</response>
    /// <response code="400">DTO inválido (validación).</response>
    /// <response code="404">Si el estudiante no existe.</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, UpdateEstudianteDto dto, CancellationToken ct)
    {
        await _service.UpdateAsync(id, dto, ct);
        return NoContent();
    }
    
    /// <summary>
    /// Elimina un estudiante.
    /// </summary>
    /// <remarks>
    /// Regla: si el estudiante tiene notas asociadas, debe responder 409 (Conflict).
    /// </remarks>
    /// <param name="id">Id del estudiante.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <response code="204">Eliminación exitosa.</response>
    /// <response code="404">Si el estudiante no existe.</response>
    /// <response code="409">Si el estudiante tiene notas asociadas.</response>
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