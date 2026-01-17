using Colegio.Web.Application.Dtos;
using Colegio.Web.Application.Intefaces;
using Colegio.Web.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Colegio.Web.Pages.Notas;

public class Edit : BasePageModel
{
    private readonly INotasService _notas;
    private readonly IProfesorService _profesores;
    private readonly IEstudianteService _estudiantes;

    public Edit(INotasService notas, IProfesorService profesores, IEstudianteService estudiantes)
    {
        _notas = notas;
        _profesores = profesores;
        _estudiantes = estudiantes;
    }

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    public List<SelectListItem> Profesores { get; private set; } = new();
    public List<SelectListItem> Estudiantes { get; private set; } = new();

    public sealed class NotaFormModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Seleccione un profesor válido.")]
        public int IdProfesor { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Seleccione un estudiante válido.")]
        public int IdEstudiante { get; set; }

        [Range(typeof(decimal), "0", "10", ErrorMessage = "El valor debe estar entre 0 y 10.")]
        public decimal Valor { get; set; }
    }

    [BindProperty]
    public NotaFormModel Form { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(CancellationToken ct)
    {
        try
        {
            await LoadCombosAsync(ct);
            
            var n = await _notas.GetByIdAsync(Id, ct); 

            Form = new NotaFormModel
            {
                Nombre = n.Nombre,
                IdProfesor = n.IdProfesor,
                IdEstudiante = n.IdEstudiante,
                Valor = n.Valor
            };

            return Page();
        }
        catch (ApiException ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToPage("Index");
        }
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        await LoadCombosAsync(ct);

        if (!ModelState.IsValid)
            return Page();

        try
        {
            var dto = new UpdateNotaDto(Form.Nombre, Form.IdProfesor, Form.IdEstudiante, Form.Valor);
            await _notas.UpdateAsync(Id, dto, ct);

            TempData["Ok"] = "Nota actualizada correctamente.";
            return RedirectToPage("Index");
        }
        catch (ApiException ex)
        {
            ApplyApiErrors(ex);
            return Page();
        }
    }

    private async Task LoadCombosAsync(CancellationToken ct)
    {
        var profs = await _profesores.GetAllAsync(ct);
        var ests  = await _estudiantes.GetAllAsync(ct);

        Profesores = profs
            .OrderBy(x => x.Nombre)
            .Select(x => new SelectListItem($"{x.Nombre} (Id: {x.Id})", x.Id.ToString()))
            .ToList();

        Estudiantes = ests
            .OrderBy(x => x.Nombre)
            .Select(x => new SelectListItem($"{x.Nombre} (Id: {x.Id})", x.Id.ToString()))
            .ToList();

        Profesores.Insert(0, new SelectListItem("Seleccione un profesor...", "0"));
        Estudiantes.Insert(0, new SelectListItem("Seleccione un estudiante...", "0"));
    }
}