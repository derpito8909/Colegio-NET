using Colegio.Domain.Common;

namespace Colegio.Domain.Entities;

public class Nota : BaseEntity
{
    public string Nombre { get; private set; } = default!;
    public int IdProfesor { get; private set; }
    public int IdEstudiante { get; private set; }
    public decimal Valor { get; private set; }
   
    public Profesor Profesor { get; private set; } = default!;
    public Estudiante Estudiante { get; private set; } = default!;

    private Nota() { }

    public Nota(string nombre, int idProfesor, int idEstudiante, decimal valor)
    {
        SetNombre(nombre);
        SetProfesor(idProfesor);
        SetEstudiante(idEstudiante);
        SetValor(valor);
    }

    public void SetNombre(string nombre)
    {
        nombre = (nombre ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre de la nota es obligatorio.");

        if (nombre.Length > 100)
            throw new ArgumentException("El nombre de la nota no puede superar 100 caracteres.");

        Nombre = nombre;
    }

    public void SetProfesor(int idProfesor)
    {
        if (idProfesor <= 0)
            throw new ArgumentException("IdProfesor debe ser un entero positivo.");

        IdProfesor = idProfesor;
    }

    public void SetEstudiante(int idEstudiante)
    {
        if (idEstudiante <= 0)
            throw new ArgumentException("IdEstudiante debe ser un entero positivo.");

        IdEstudiante = idEstudiante;
    }

    public void SetValor(decimal valor)
    {
        if (valor < 0 || valor > 10)
            throw new ArgumentException("El valor de la nota debe estar entre 0 y 10.");

        Valor = valor;
    }
}