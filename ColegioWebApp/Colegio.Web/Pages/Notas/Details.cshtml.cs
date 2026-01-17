using Colegio.Web.Application.Dtos;
using Colegio.Web.Application.Intefaces;
using Colegio.Web.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Colegio.Web.Pages.Notas;

public class Details : BasePageModel
{
    private readonly INotasService _notas;

    public Details(INotasService notas) => _notas = notas;

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    public NotaDetalleDto Nota { get; private set; } = default!;

    public async Task<IActionResult> OnGetAsync(CancellationToken ct)
    {
        try
        {
            Nota = await _notas.GetByIdDetalleAsync(Id, ct);
            return Page();
        }
        catch (ApiException ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToPage("Index");
        }
    }
}