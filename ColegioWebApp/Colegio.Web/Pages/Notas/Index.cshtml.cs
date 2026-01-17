using Colegio.Web.Application.Services;
using Colegio.Web.Application.Dtos;
using Colegio.Web.Application.Intefaces;
using Colegio.Web.Infrastructure.Exceptions;

namespace Colegio.Web.Pages.Notas;

public class Index : BasePageModel
{
    private readonly INotasService _notas;

    public Index(INotasService notas) => _notas = notas;

    public PagedResult<NotaDetalleDto> PageData { get; private set; } = default!;

    public async Task OnGetAsync(int page = 1, int pageSize = 10, CancellationToken ct = default)
    {
        var all = await _notas.GetAllDetalleAsync(ct);
        PageData = PagedResult<NotaDetalleDto>.From(all, page, pageSize);
    }

    public async Task OnPostDeleteAsync(int id, int page = 1, int pageSize = 10, CancellationToken ct = default)
    {
        try
        {
            await _notas.DeleteAsync(id, ct);
            TempData["Ok"] = "Nota eliminada correctamente.";
        }
        catch (ApiException ex)
        {
            ApplyApiErrorToTempData(ex);
        }

        await OnGetAsync(page, pageSize, ct);
    }
}