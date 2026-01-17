using Colegio.Web.Application.Dtos;
using Colegio.Web.Application.Intefaces;
using Colegio.Web.Infrastructure.Exceptions;

namespace Colegio.Web.Pages;

public class IndexModel : BasePageModel
{
    private readonly IEstudianteService _estudiantes;
    private readonly IProfesorService _profesores;
    private readonly INotasService _notas;

    public IndexModel(
        IEstudianteService estudiantes,
        IProfesorService profesores,
        INotasService notas)
    {
        _estudiantes = estudiantes;
        _profesores = profesores;
        _notas = notas;
    }

    public int TotalEstudiantes { get; private set; }
    public int TotalProfesores { get; private set; }
    public int TotalNotas { get; private set; }

    public List<NotaDetalleDto> UltimasNotas { get; private set; } = new();

    public async Task OnGetAsync(CancellationToken ct)
    {
        try
        {
            var estudiantesTask = _estudiantes.GetAllAsync(ct);
            var profesoresTask = _profesores.GetAllAsync(ct);
            var notasTask = _notas.GetAllDetalleAsync(ct);

            await Task.WhenAll(estudiantesTask, profesoresTask, notasTask);

            var estudiantes = await estudiantesTask;
            var profesores = await profesoresTask;
            var notas = await notasTask;

            TotalEstudiantes = estudiantes.Count;
            TotalProfesores = profesores.Count;
            TotalNotas = notas.Count;
            
            UltimasNotas = notas
                .OrderByDescending(n => n.Id)
                .Take(5)
                .ToList();
        }
        catch (ApiException ex)
        {
            ApplyApiErrorToTempData(ex);
            
            TotalEstudiantes = 0;
            TotalProfesores = 0;
            TotalNotas = 0;
            UltimasNotas = new();
        }
    }
}
