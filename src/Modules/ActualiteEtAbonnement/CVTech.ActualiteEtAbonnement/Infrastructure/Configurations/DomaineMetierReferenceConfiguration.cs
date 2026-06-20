using CVTech.ActualiteEtAbonnement.Domaine;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CVTech.ActualiteEtAbonnement.Infrastructure.Configurations;

public sealed class DomaineMetierReferenceConfiguration : IEntityTypeConfiguration<DomaineMetierReference>
{
    public void Configure(EntityTypeBuilder<DomaineMetierReference> builder)
    {
        builder.ToTable("DomainesMetier");
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Code).HasMaxLength(64).IsRequired();
        builder.HasIndex(d => d.Code).IsUnique();

        builder.Property(d => d.Libelle).HasMaxLength(128).IsRequired();
        builder.Property(d => d.DateCreation).IsRequired();
    }
}
