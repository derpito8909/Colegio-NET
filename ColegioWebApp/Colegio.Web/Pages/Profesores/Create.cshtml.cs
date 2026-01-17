using Colegio.Web.Application.Dtos;
using Colegio.Web.Application.Intefaces;
using Colegio.Web.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Colegio.Web.Pages.Profesores;

public class Create : BasePageModel
{
    private readonly IProfesorService _service;

    public Create(IProfesorService service) => _service = service;

    public sealed class ProfesorFormModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;
    }

    [BindProperty]
    public ProfesorFormModel Form { get; set; } = new();

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        if (!ModelState.IsValid) return Page();

        try
        {
            await _service.CreateAsync(new CreateProfesorDto(Form.Nombre), ct);
            TempData["Ok"] = "Profesor creado correctamente.";
            return RedirectToPage("Index");
        }
        catch (ApiException ex)
        {
            ApplyApiErrors(ex);
            return Page();
        }
    }
}