using Colegio.Application.Dtos;

namespace Colegio.Application.Interfaces;

public interface IProfesorService
{
    Task<IReadOnlyList<ProfesorDto>> GetAllAsync(CancellationToken ct);
    Task<ProfesorDto> GetByIdAsync(int id, CancellationToken ct);

    Task<ProfesorDto> CreateAsync(CreateProfesorDto dto, CancellationToken ct);
    Task UpdateAsync(int id, UpdateProfesorDto dto, CancellationToken ct);
    Task DeleteAsync(int id, CancellationToken ct);
}