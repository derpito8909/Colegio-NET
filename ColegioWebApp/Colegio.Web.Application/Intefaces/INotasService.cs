using Colegio.Web.Application.Dtos;

namespace Colegio.Web.Application.Intefaces;

public interface INotasService
{
    Task<IReadOnlyList<NotaDto>> GetAllAsync(CancellationToken ct);
    Task<IReadOnlyList<NotaDetalleDto>> GetAllDetalleAsync(CancellationToken ct);

    Task<NotaDto> GetByIdAsync(int id, CancellationToken ct);
    Task<NotaDetalleDto> GetByIdDetalleAsync(int id, CancellationToken ct);

    Task CreateAsync(CreateNotaDto dto, CancellationToken ct);
    Task UpdateAsync(int id, UpdateNotaDto dto, CancellationToken ct);
    Task DeleteAsync(int id, CancellationToken ct);
}