using CVTech.ActualiteEtAbonnement.Domaine;
using Microsoft.EntityFrameworkCore;

namespace CVTech.ActualiteEtAbonnement.Infrastructure;

public sealed class ActualiteEtAbonnementDbContext : DbContext
{
    public ActualiteEtAbonnementDbContext(DbContextOptions<ActualiteEtAbonnementDbContext> options) : base(options)
    {
    }

    public DbSet<DomaineMetierReference> DomainesMetier => Set<DomaineMetierReference>();
    public DbSet<ArticleActualite> Articles => Set<ArticleActualite>();
    public DbSet<Abonnement> Abonnements => Set<Abonnement>();
    public DbSet<Notification> Notifications => Set<Notification>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ActualiteEtAbonnementDbContext).Assembly);
    }
}
