using Colegio.Application.Interfaces;
using Colegio.Infrastructure.Persistence;
using Colegio.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Colegio.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var conn = configuration.GetConnectionString("DefaultConnection")
                   ?? throw new InvalidOperationException("ConnectionString 'DefaultConnection' no configurada.");

        services.AddDbContext<ColegioDbContext>(opt =>
            opt.UseSqlServer(conn));
        
        services.AddScoped<IEstudianteService, EstudianteService>();
        services.AddScoped<IProfesorService, ProfesorService>();
        services.AddScoped<INotaService, NotaService>();

        return services;
    }
}