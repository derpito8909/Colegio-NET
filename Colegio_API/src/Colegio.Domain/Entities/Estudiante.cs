using Colegio.Domain.Common;

namespace Colegio.Domain.Entities;

public class Estudiante : BaseEntity
{
    public string Nombre { get; private set; } = default!;
    
    private Estudiante() { }

    public Estudiante(string nombre)
    {
        SetNombre(nombre);
    }

    public void SetNombre(string nombre)
    {
        nombre = (nombre ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre del estudiante es obligatorio.");

        if (nombre.Length > 100)
            throw new ArgumentException("El nombre del estudiante no puede superar 100 caracteres.");

        Nombre = nombre;
    }
    
    public ICollection<Nota> Notas { get; private set; } = new List<Nota>();
    
}