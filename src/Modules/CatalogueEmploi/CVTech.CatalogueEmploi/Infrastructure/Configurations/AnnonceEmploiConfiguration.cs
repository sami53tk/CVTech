using CVTech.CatalogueEmploi.Domaine;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CVTech.CatalogueEmploi.Infrastructure.Configurations;

public sealed class AnnonceEmploiConfiguration : IEntityTypeConfiguration<AnnonceEmploi>
{
    public void Configure(EntityTypeBuilder<AnnonceEmploi> builder)
    {
        builder.ToTable("Annonces");
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Titre).HasMaxLength(256).IsRequired();
        builder.Property(a => a.Description).IsRequired();
        builder.Property(a => a.TypeContrat).HasConversion<string>().HasMaxLength(32).IsRequired();
        builder.Property(a => a.DomaineMetierCode).HasMaxLength(64).IsRequired();
        builder.Property(a => a.Localisation).HasMaxLength(128).IsRequired();
        builder.Property(a => a.EntrepriseId).IsRequired();
        builder.Property(a => a.EstSupprimee).IsRequired();
        builder.Property(a => a.DateCreation).IsRequired();
    }
}
