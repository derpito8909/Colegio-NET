using Colegio.Application.Exceptions;
using Colegio.Application.Dtos;
using Colegio.Application.Interfaces;
using Colegio.Domain.Entities;
using Colegio.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Colegio.Infrastructure.Services;

public sealed class ProfesorService : IProfesorService
{
    private readonly ColegioDbContext _db;

    public ProfesorService(ColegioDbContext db) => _db = db;

    public async Task<IReadOnlyList<ProfesorDto>> GetAllAsync(CancellationToken ct)
        => await _db.Profesores
            .AsNoTracking()
            .Select(p => new ProfesorDto(p.Id, p.Nombre))
            .ToListAsync(ct);

    public async Task<ProfesorDto> GetByIdAsync(int id, CancellationToken ct)
    {
        var dto = await _db.Profesores
            .AsNoTracking()
            .Where(p => p.Id == id)
            .Select(p => new ProfesorDto(p.Id, p.Nombre))
            .FirstOrDefaultAsync(ct);

        return dto ?? throw NotFoundException.For("Profesor", id);
    }

    public async Task<ProfesorDto> CreateAsync(CreateProfesorDto dto, CancellationToken ct)
    {
        var entity = new Profesor(dto.Nombre);
        _db.Profesores.Add(entity);
        await _db.SaveChangesAsync(ct);

        return new ProfesorDto(entity.Id, entity.Nombre);
    }

    public async Task UpdateAsync(int id, UpdateProfesorDto dto, CancellationToken ct)
    {
        var entity = await _db.Profesores.FirstOrDefaultAsync(p => p.Id == id, ct)
                     ?? throw NotFoundException.For("Profesor", id);

        entity.SetNombre(dto.Nombre);
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct)
    {
        var entity = await _db.Profesores.FirstOrDefaultAsync(p => p.Id == id, ct)
                     ?? throw NotFoundException.For("Profesor", id);

        var tieneNotas = await _db.Notas.AsNoTracking().AnyAsync(n => n.IdProfesor == id, ct);
        if (tieneNotas)
            throw new ConflictException("No se puede eliminar al profesor porque tiene notas asociadas.");

        _db.Profesores.Remove(entity);
        await _db.SaveChangesAsync(ct);
    }
}