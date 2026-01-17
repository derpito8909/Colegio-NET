using Colegio.Web.Application.Dtos;
using Colegio.Web.Application.Intefaces;

namespace Colegio.Web.Infrastructure.Services;

public sealed class EstudianteService : IEstudianteService
{
    private readonly ColegioApiClient _api;
    public EstudianteService(ColegioApiClient api) => _api = api;

    public Task<IReadOnlyList<EstudianteDto>> GetAllAsync(CancellationToken ct)
        => _api.GetAsync<IReadOnlyList<EstudianteDto>>("api/Estudiante", ct);

    public Task<EstudianteDto> GetByIdAsync(int id, CancellationToken ct)
        => _api.GetAsync<EstudianteDto>($"api/Estudiante/{id}", ct);

    public Task CreateAsync(CreateEstudianteDto dto, CancellationToken ct)
        => _api.PostAsync("api/Estudiante", dto, ct);

    public Task UpdateAsync(int id, UpdateEstudianteDto dto, CancellationToken ct)
        => _api.PutAsync($"api/Estudiante/{id}", dto, ct);

    public Task DeleteAsync(int id, CancellationToken ct)
        => _api.DeleteAsync($"api/Estudiante/{id}", ct);
}