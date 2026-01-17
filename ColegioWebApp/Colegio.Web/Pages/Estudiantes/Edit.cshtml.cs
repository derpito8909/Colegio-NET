using Colegio.Web.Application.Dtos;
using Colegio.Web.Application.Intefaces;
using Colegio.Web.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Colegio.Web.Pages.Estudiantes;

public class Edit : BasePageModel
{
    private readonly IEstudianteService _service;
    public Edit(IEstudianteService service) => _service = service;

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    [BindProperty]
    public UpdateEstudianteDto Form { get; set; } = new("");

    public async Task<IActionResult> OnGetAsync(CancellationToken ct)
    {
        try
        {
            var e = await _service.GetByIdAsync(Id, ct);
            Form = new UpdateEstudianteDto(e.Nombre);
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
        if (!ModelState.IsValid) return Page();

        try
        {
            await _service.UpdateAsync(Id, Form, ct);
            TempData["Ok"] = "Estudiante actualizado correctamente.";
            return RedirectToPage("Index");
        }
        catch (ApiException ex)
        {
            ApplyApiErrors(ex);
            return Page();
        }
    }
}