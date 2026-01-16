using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Colegio.Application.Exceptions;
using Microsoft.Data.SqlClient;

namespace Colegio.Api.Middleware;

/// <summary>
/// Middleware global de manejo de excepciones.
/// Centraliza la conversión de excepciones del sistema en respuestas HTTP consistentes
/// usando el formato estándar <see cref="ProblemDetails"/>.
/// </summary>
/// <remarks>
/// Beneficios:
/// - Evita repetir try/catch en controllers.
/// - Respuestas uniformes (404/409/500).
/// - Logging centralizado.
/// </remarks>
public sealed class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    /// <summary>
    /// Construye el middleware con un logger inyectado por DI.
    /// </summary>
    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
        => _logger = logger;

    /// <summary>
    /// Ejecuta el middleware para cada request HTTP.
    /// Si ocurre una excepción, se genera una respuesta JSON tipo "application/problem+json".
    /// </summary>
    /// <param name="context">Contexto de la petición actual.</param>
    /// <param name="next">Delegado para continuar el pipeline.</param>
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            var errors = ex.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage).ToArray());

            var problem = new ValidationProblemDetails(errors)
            {
                Title = "La solicitud tiene errores de validación.",
                Status = StatusCodes.Status400BadRequest,
                Type = "https://httpstatuses.com/400",
                Instance = context.Request.Path
            };

            await WriteProblemAsync(context, problem, StatusCodes.Status400BadRequest);
        }
        catch (NotFoundException ex)
        {
            var problem = new ProblemDetails
            {
                Status = 404,
                Title = "No encontrado.",
                Detail = ex.Message,
                Instance = context.Request.Path
            };

            context.Response.StatusCode = 404;
            context.Response.ContentType = "application/problem+json";
            await context.Response.WriteAsJsonAsync(problem);
        }
        catch (ArgumentException ex)
        {
            var problem = NewProblem(context, 400, "Solicitud inválida.", ex.Message);
            await WriteProblemAsync(context, problem, 400);
        }
        catch (BusinessRuleException ex)
        {
            var problem = NewProblem(context, 400, "Regla de negocio no cumplida.", ex.Message);
            await WriteProblemAsync(context, problem, 400);
        }
        catch (ConflictException ex)
        {
            _logger.LogWarning(ex, "Conflicto de negocio.");
            var problem = NewProblem(context, 409, "Conflicto.", ex.Message);
            await WriteProblemAsync(context, problem, 409);
        }
        catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 547)
        {
            var detail = "La operación viola una regla de integridad en la base de datos.";
            
            if (sqlEx.Message.Contains("FK_Nota_Estudiante", StringComparison.OrdinalIgnoreCase))
                detail = "No se puede eliminar el estudiante porque tiene notas asociadas.";
            else if (sqlEx.Message.Contains("FK_Nota_Profesor", StringComparison.OrdinalIgnoreCase))
                detail = "No se puede eliminar el profesor porque tiene notas asociadas.";

            var problem = NewProblem(context, 409, "Conflicto.", detail);
            await WriteProblemAsync(context, problem, 409);
        }
        catch (DbUpdateException)
        {
            var problem = NewProblem(context, 409, "Conflicto.", "La operación viola una regla de integridad en la base de datos.");
            await WriteProblemAsync(context, problem, 409);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error no controlado.");
            var problem = NewProblem(context, 500, "Error interno.", "Ocurrió un error inesperado.");
            await WriteProblemAsync(context, problem, 500);
        }
    }

    private static ProblemDetails NewProblem(HttpContext ctx, int status, string title, string detail)
        => new()
        {
            Status = status,
            Title = title,
            Detail = detail,
            Type = $"https://httpstatuses.com/{status}",
            Instance = ctx.Request.Path
        };

    private static async Task WriteProblemAsync(HttpContext ctx, object problem, int status)
    {
        if (ctx.Response.HasStarted) return;

        ctx.Response.Clear();
        ctx.Response.StatusCode = status;
        ctx.Response.ContentType = "application/problem+json";
        await ctx.Response.WriteAsJsonAsync(problem);
    }
}