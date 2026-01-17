using Colegio.Web.Application.Intefaces;
using Colegio.Web.Infrastructure.Services;

namespace Colegio.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddWeb(this IServiceCollection services, IConfiguration config)
    {
        services.AddRazorPages();

        services.AddHttpClient<ColegioApiClient>(client =>
        {
            var baseUrl = config["Api:BaseUrl"]!;
            client.BaseAddress = new Uri(baseUrl);
        });

        services.AddScoped<IEstudianteService, EstudianteService>();
        services.AddScoped<IProfesorService, ProfesorService>();
        services.AddScoped<INotasService, NotaService>();

        return services;
    }
}