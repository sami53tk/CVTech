using CVTech.GestionIdentite.Domaine;
using Microsoft.EntityFrameworkCore;

namespace CVTech.GestionIdentite.Infrastructure;

public sealed class IdentiteDbContext : DbContext
{
    public IdentiteDbContext(DbContextOptions<IdentiteDbContext> options) : base(options)
    {
    }

    public DbSet<Utilisateur> Utilisateurs => Set<Utilisateur>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IdentiteDbContext).Assembly);
    }
}
