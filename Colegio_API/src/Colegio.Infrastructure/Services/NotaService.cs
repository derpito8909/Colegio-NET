using Colegio.Application.Dtos;
using Colegio.Application.Interfaces;
using Colegio.Domain.Entities;
using Colegio.Infrastructure.Persistence;
using Colegio.Application.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Colegio.Infrastructure.Services;

public sealed class NotaService : INotaService
{
    private readonly ColegioDbContext _db;

    public NotaService(ColegioDbContext db) => _db = db;

    /// <inheritdoc />
    public async Task<IReadOnlyList<NotaDto>> GetAllAsync(CancellationToken ct)
        => await _db.Notas
            .AsNoTracking()
            .Select(n => new NotaDto(n.Id, n.Nombre, n.IdProfesor, n.IdEstudiante, n.Valor))
            .ToListAsync(ct);

    public async Task<IReadOnlyList<NotaDetalleDto>> GetAllDetailAsync(CancellationToken ct)
        => await _db.Notas
            .AsNoTracking()
            .Select(n => new NotaDetalleDto(
                n.Id,
                n.Nombre,
                n.Valor,
                n.IdProfesor,
                n.Profesor.Nombre,
                n.IdEstudiante,
                n.Estudiante.Nombre
            ))
            .ToListAsync(ct);

    public async Task<NotaDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        var dto = await _db.Notas
            .AsNoTracking()
            .Where(n => n.Id == id)
            .Select(n => new NotaDto(n.Id, n.Nombre, n.IdProfesor, n.IdEstudiante, n.Valor))
            .FirstOrDefaultAsync(ct);
        return dto ?? throw NotFoundException.For("Nota", id);
    }

    public async Task<NotaDetalleDto?> GetByIdDetalleAsync(int id, CancellationToken ct)
    {
        var dto = await _db.Notas
            .AsNoTracking()
            .Where(n => n.Id == id)
            .Select(n => new NotaDetalleDto(
                n.Id,
                n.Nombre,
                n.Valor,
                n.IdProfesor,
                n.Profesor.Nombre,
                n.IdEstudiante,
                n.Estudiante.Nombre
            ))
            .FirstOrDefaultAsync(ct);
        return dto ?? throw NotFoundException.For("Nota", id);
    } 

    public async Task<NotaDto> CreateAsync(CreateNotaDto dto, CancellationToken ct)
    {
        var profesorExiste = await _db.Profesores.AnyAsync(p => p.Id == dto.IdProfesor, ct);
        if (!profesorExiste) throw NotFoundException.For("Profesor", dto.IdProfesor);

        var estudianteExiste = await _db.Estudiantes.AnyAsync(e => e.Id == dto.IdEstudiante, ct);
        if (!estudianteExiste) throw NotFoundException.For("Estudiante", dto.IdEstudiante);

        var nota = new Nota(dto.Nombre, dto.IdProfesor, dto.IdEstudiante, dto.Valor);

        _db.Notas.Add(nota);
        await _db.SaveChangesAsync(ct);

        return new NotaDto(nota.Id, nota.Nombre, nota.IdProfesor, nota.IdEstudiante, nota.Valor);
    }

    public async Task<bool> UpdateAsync(int id, UpdateNotaDto dto, CancellationToken ct)
    {
        var nota = await _db.Notas.FirstOrDefaultAsync(n => n.Id == id, ct);
        if (nota is null) throw NotFoundException.For("Nota", id);

        var profesorExiste = await _db.Profesores.AnyAsync(p => p.Id == dto.IdProfesor, ct);
        if (!profesorExiste) throw NotFoundException.For("Profesor", dto.IdProfesor);

        var estudianteExiste = await _db.Estudiantes.AnyAsync(e => e.Id == dto.IdEstudiante, ct);
        if (!estudianteExiste) throw NotFoundException.For("Estudiante", dto.IdEstudiante);

        nota.SetNombre(dto.Nombre);
        nota.SetProfesor(dto.IdProfesor);
        nota.SetEstudiante(dto.IdEstudiante);
        nota.SetValor(dto.Valor);

        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var nota = await _db.Notas.FirstOrDefaultAsync(n => n.Id == id, ct);
        if (nota is null) throw NotFoundException.For("Nota", id);

        _db.Notas.Remove(nota);
        await _db.SaveChangesAsync(ct);
        return true;
    }
}