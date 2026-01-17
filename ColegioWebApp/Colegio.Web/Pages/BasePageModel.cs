using Colegio.Web.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Colegio.Web.Pages;

public abstract class BasePageModel : PageModel
{
    protected void ApplyApiErrors(ApiException ex)
    {
        if (ex.StatusCode == 400 && ex.Problem?.Errors is not null && ex.Problem.Errors.Count > 0)
        {
            foreach (var (field, messages) in ex.Problem.Errors)
            {
                foreach (var msg in messages)
                {
                    // directo
                    ModelState.AddModelError(field, msg);

                    // prefijo Form.
                    if (!field.StartsWith("Form.", StringComparison.OrdinalIgnoreCase))
                        ModelState.AddModelError($"Form.{field}", msg);
                }
            }
            
            ModelState.AddModelError(string.Empty, "La solicitud tiene errores. Revisa los campos.");
            return;
        }

        
        if (ex.StatusCode == 404)
        {
            TempData["Warn"] = ex.Message; 
            return;
        }

        if (ex.StatusCode == 409)
        {
            TempData["Warn"] = ex.Message;
            return;
        }
        
        TempData["Error"] = ex.Message;
    }

    protected void ApplyApiErrorToTempData(ApiException ex)
    {
        if (ex.StatusCode is 404 or 409) TempData["Warn"] = ex.Message;
        else TempData["Error"] = ex.Message;
    }
}