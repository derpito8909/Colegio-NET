using Colegio.Application.Exceptions;
using Colegio.Application.Dtos;
using Colegio.Application.Interfaces;
using Colegio.Domain.Entities;
using Colegio.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Colegio.Infrastructure.Services;

public sealed class EstudianteService : IEstudianteService
{
    private readonly ColegioDbContext _db;

    public EstudianteService(ColegioDbContext db) => _db = db;

    public async Task<IReadOnlyList<EstudianteDto>> GetAllAsync(CancellationToken ct)
        => await _db.Estudiantes
            .AsNoTracking()
            .Select(e => new EstudianteDto(e.Id, e.Nombre))
            .ToListAsync(ct);

    public async Task<EstudianteDto> GetByIdAsync(int id, CancellationToken ct)
    {
        var dto = await _db.Estudiantes
            .AsNoTracking()
            .Where(e => e.Id == id)
            .Select(e => new EstudianteDto(e.Id, e.Nombre))
            .FirstOrDefaultAsync(ct);

        return dto ?? throw NotFoundException.For("Estudiante", id);
    }

    public async Task<EstudianteDto> CreateAsync(CreateEstudianteDto dto, CancellationToken ct)
    {
        var entity = new Estudiante(dto.Nombre);
        _db.Estudiantes.Add(entity);
        await _db.SaveChangesAsync(ct);

        return new EstudianteDto(entity.Id, entity.Nombre);
    }

    public async Task UpdateAsync(int id, UpdateEstudianteDto dto, CancellationToken ct)
    {
        var entity = await _db.Estudiantes.FirstOrDefaultAsync(e => e.Id == id, ct)
                     ?? throw NotFoundException.For("Estudiante", id);

        entity.SetNombre(dto.Nombre);
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct)
    {
        var entity = await _db.Estudiantes.FirstOrDefaultAsync(e => e.Id == id, ct)
                     ?? throw NotFoundException.For("Estudiante", id);
        
        var tieneNotas = await _db.Notas.AsNoTracking().AnyAsync(n => n.IdEstudiante == id, ct);
        if (tieneNotas)
            throw new ConflictException("No se puede eliminar al estudiante porque tiene notas asociadas.");

        _db.Estudiantes.Remove(entity);
        await _db.SaveChangesAsync(ct);
    }
}