using Colegio.Web.Application.Dtos;
using Colegio.Web.Application.Intefaces;
using Colegio.Web.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Colegio.Web.Pages.Estudiantes;

public class Create : BasePageModel
{
    private readonly IEstudianteService _service;
    public Create(IEstudianteService service) => _service = service;

    [BindProperty]
    public CreateEstudianteDto Form { get; set; } = new("");

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        if (!ModelState.IsValid) return Page();

        try
        {
            await _service.CreateAsync(Form, ct);
            TempData["Ok"] = "Estudiante creado correctamente.";
            return RedirectToPage("Index");
        }
        catch (ApiException ex)
        {
            ApplyApiErrors(ex);
            return Page();
        }
    }
}