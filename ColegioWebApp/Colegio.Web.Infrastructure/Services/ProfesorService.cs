using Colegio.Web.Application.Dtos;
using Colegio.Web.Application.Intefaces;

namespace Colegio.Web.Infrastructure.Services;

public sealed class ProfesorService : IProfesorService
{
    private readonly ColegioApiClient _api;
    public ProfesorService(ColegioApiClient api) => _api = api;

    public Task<IReadOnlyList<ProfesorDto>> GetAllAsync(CancellationToken ct)
        => _api.GetAsync<IReadOnlyList<ProfesorDto>>("api/Profesor", ct);

    public Task<ProfesorDto> GetByIdAsync(int id, CancellationToken ct)
        => _api.GetAsync<ProfesorDto>($"api/Profesor/{id}", ct);

    public Task CreateAsync(CreateProfesorDto dto, CancellationToken ct)
        => _api.PostAsync("api/Profesor", dto, ct);

    public Task UpdateAsync(int id, UpdateProfesorDto dto, CancellationToken ct)
        => _api.PutAsync($"api/Profesor/{id}", dto, ct);

    public Task DeleteAsync(int id, CancellationToken ct)
        => _api.DeleteAsync($"api/Profesor/{id}", ct);
}