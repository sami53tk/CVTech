using CVTech.AppelOffreFreelance.Domaine;
using Microsoft.EntityFrameworkCore;

namespace CVTech.AppelOffreFreelance.Infrastructure;

public sealed class AppelOffreFreelanceDbContext : DbContext
{
    public AppelOffreFreelanceDbContext(DbContextOptions<AppelOffreFreelanceDbContext> options)
        : base(options) { }

    public DbSet<AppelOffre> AppelsOffres => Set<AppelOffre>();
    public DbSet<PropositionFreelance> Propositions => Set<PropositionFreelance>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppelOffreFreelanceDbContext).Assembly);
    }
}
