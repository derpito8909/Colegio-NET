using Colegio.Web.Application.Dtos;

namespace Colegio.Web.Application.Intefaces;

public interface IProfesorService
{
    Task<IReadOnlyList<ProfesorDto>> GetAllAsync(CancellationToken ct);
    Task<ProfesorDto> GetByIdAsync(int id, CancellationToken ct);

    Task CreateAsync(CreateProfesorDto dto, CancellationToken ct);
    Task UpdateAsync(int id, UpdateProfesorDto dto, CancellationToken ct);
    Task DeleteAsync(int id, CancellationToken ct);
}