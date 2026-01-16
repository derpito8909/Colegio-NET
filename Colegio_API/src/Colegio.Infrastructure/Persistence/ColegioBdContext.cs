using Colegio.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Colegio.Infrastructure.Persistence;

public class ColegioDbContext : DbContext
{
    public ColegioDbContext(DbContextOptions<ColegioDbContext> options) : base(options) { }

    public DbSet<Estudiante> Estudiantes => Set<Estudiante>();
    public DbSet<Profesor> Profesores => Set<Profesor>();
    public DbSet<Nota> Notas => Set<Nota>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Estudiante>(e =>
        {
            e.ToTable("Estudiante");
            e.HasKey(x => x.Id);

            e.Property(x => x.Nombre)
             .IsRequired()
             .HasMaxLength(100);
        });

        modelBuilder.Entity<Profesor>(e =>
        {
            e.ToTable("Profesor");
            e.HasKey(x => x.Id);

            e.Property(x => x.Nombre)
             .IsRequired()
             .HasMaxLength(100);
        });

        modelBuilder.Entity<Nota>(e =>
        {
            e.ToTable("Nota");
            e.HasKey(x => x.Id);

            e.Property(x => x.Nombre)
             .IsRequired()
             .HasMaxLength(100);

            e.Property(x => x.Valor)
             .HasPrecision(4, 2)
             .IsRequired();
           
            e.ToTable(t => t.HasCheckConstraint("CK_Nota_Valor", "[Valor] >= 0 AND [Valor] <= 10"));

            e.HasIndex(x => x.IdEstudiante).HasDatabaseName("IX_Nota_IdEstudiante");
            e.HasIndex(x => x.IdProfesor).HasDatabaseName("IX_Nota_IdProfesor");

            e.HasOne(x => x.Estudiante)
             .WithMany(x => x.Notas)
             .HasForeignKey(x => x.IdEstudiante)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.Profesor)
             .WithMany(x => x.Notas)
             .HasForeignKey(x => x.IdProfesor)
             .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
