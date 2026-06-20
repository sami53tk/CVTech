using CVTech.ActualiteEtAbonnement.Domaine;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CVTech.ActualiteEtAbonnement.Infrastructure.Configurations;

public sealed class AbonnementConfiguration : IEntityTypeConfiguration<Abonnement>
{
    public void Configure(EntityTypeBuilder<Abonnement> builder)
    {
        builder.ToTable("Abonnements");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.UtilisateurId).IsRequired();
        builder.Property(a => a.DomaineMetierCode).HasMaxLength(64).IsRequired();
        builder.Property(a => a.DateCreation).IsRequired();
        builder.HasIndex(a => new { a.UtilisateurId, a.DomaineMetierCode }).IsUnique();
    }
}
