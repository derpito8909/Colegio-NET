using Colegio.Web.Application.Dtos;
using Colegio.Web.Application.Intefaces;

namespace Colegio.Web.Infrastructure.Services;

public sealed class NotaService : INotasService
{
    private readonly ColegioApiClient _api;
    public NotaService(ColegioApiClient api) => _api = api;

    public Task<IReadOnlyList<NotaDto>> GetAllAsync(CancellationToken ct)
        => _api.GetAsync<IReadOnlyList<NotaDto>>("api/Notas", ct);

    public Task<IReadOnlyList<NotaDetalleDto>> GetAllDetalleAsync(CancellationToken ct)
        => _api.GetAsync<IReadOnlyList<NotaDetalleDto>>("api/Notas/detalle", ct);

    public Task<NotaDto> GetByIdAsync(int id, CancellationToken ct)
        => _api.GetAsync<NotaDto>($"api/Notas/{id}", ct);

    public Task<NotaDetalleDto> GetByIdDetalleAsync(int id, CancellationToken ct)
        => _api.GetAsync<NotaDetalleDto>($"api/Notas/{id}/detalle", ct);

    public Task CreateAsync(CreateNotaDto dto, CancellationToken ct)
        => _api.PostAsync("api/Notas", dto, ct);

    public Task UpdateAsync(int id, UpdateNotaDto dto, CancellationToken ct)
        => _api.PutAsync($"api/Notas/{id}", dto, ct);

    public Task DeleteAsync(int id, CancellationToken ct)
        => _api.DeleteAsync($"api/Notas/{id}", ct);
}