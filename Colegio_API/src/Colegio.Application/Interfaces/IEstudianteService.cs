using Colegio.Application.Dtos;

namespace Colegio.Application.Interfaces;

public interface IEstudianteService
{
    Task<IReadOnlyList<EstudianteDto>> GetAllAsync(CancellationToken ct);
    Task<EstudianteDto> GetByIdAsync(int id, CancellationToken ct);

    Task<EstudianteDto> CreateAsync(CreateEstudianteDto dto, CancellationToken ct);
    Task UpdateAsync(int id, UpdateEstudianteDto dto, CancellationToken ct);
    Task DeleteAsync(int id, CancellationToken ct);
}