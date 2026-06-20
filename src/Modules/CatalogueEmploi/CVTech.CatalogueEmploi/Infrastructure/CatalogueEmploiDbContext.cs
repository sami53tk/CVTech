using CVTech.CatalogueEmploi.Domaine;
using Microsoft.EntityFrameworkCore;

namespace CVTech.CatalogueEmploi.Infrastructure;

public sealed class CatalogueEmploiDbContext : DbContext
{
    public CatalogueEmploiDbContext(DbContextOptions<CatalogueEmploiDbContext> options) : base(options)
    {
    }

    public DbSet<AnnonceEmploi> Annonces => Set<AnnonceEmploi>();

    public DbSet<CurriculumVitae> CurriculumsVitae => Set<CurriculumVitae>();

    public DbSet<Candidature> Candidatures => Set<Candidature>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogueEmploiDbContext).Assembly);
    }
}
