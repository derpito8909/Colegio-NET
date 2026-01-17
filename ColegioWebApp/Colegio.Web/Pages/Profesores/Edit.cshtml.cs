using Colegio.Web.Application.Dtos;
using Colegio.Web.Application.Intefaces;
using Colegio.Web.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Colegio.Web.Pages.Profesores;

public class Edit : BasePageModel
{
    private readonly IProfesorService _service;

    public Edit(IProfesorService service) => _service = service;

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    public sealed class ProfesorFormModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;
    }

    [BindProperty]
    public ProfesorFormModel Form { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(CancellationToken ct)
    {
        try
        {
            var p = await _service.GetByIdAsync(Id, ct);
            Form = new ProfesorFormModel { Nombre = p.Nombre };
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
            await _service.UpdateAsync(Id, new UpdateProfesorDto(Form.Nombre), ct);
            TempData["Ok"] = "Profesor actualizado correctamente.";
            return RedirectToPage("Index");
        }
        catch (ApiException ex)
        {
            ApplyApiErrors(ex);
            return Page();
        }
    }
}