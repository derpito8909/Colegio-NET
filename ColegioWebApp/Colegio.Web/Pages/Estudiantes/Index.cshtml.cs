
using Colegio.Web.Application.Services;
using Colegio.Web.Application.Dtos;
using Colegio.Web.Application.Intefaces;
using Colegio.Web.Infrastructure.Exceptions;

namespace Colegio.Web.Pages.Estudiantes;

public class IndexModel : BasePageModel
{
    private readonly IEstudianteService _service;
    public IndexModel(IEstudianteService service) => _service = service;

    public PagedResult<EstudianteDto> PageData { get; private set; } = default!;

    public async Task OnGetAsync(int page = 1, int pageSize = 10, CancellationToken ct = default)
    {
        var all = await _service.GetAllAsync(ct);
        PageData = PagedResult<EstudianteDto>.From(all, page, pageSize);
    }

    public async Task OnPostDeleteAsync(int id, int page = 1, int pageSize = 10, CancellationToken ct = default)
    {
        try
        {
            await _service.DeleteAsync(id, ct);
            TempData["Ok"] = "Estudiante eliminado correctamente.";
        }
        catch (ApiException ex)
        {
            ApplyApiErrorToTempData(ex);
        }

        await OnGetAsync(page, pageSize, ct);
    }
}