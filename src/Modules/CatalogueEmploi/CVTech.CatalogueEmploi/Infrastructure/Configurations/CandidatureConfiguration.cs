using CVTech.CatalogueEmploi.Domaine;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CVTech.CatalogueEmploi.Infrastructure.Configurations;

public sealed class CandidatureConfiguration : IEntityTypeConfiguration<Candidature>
{
    public void Configure(EntityTypeBuilder<Candidature> builder)
    {
        builder.ToTable("Candidatures");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.AnnonceEmploiId).IsRequired();
        builder.Property(c => c.CandidatId).IsRequired();
        builder.Property(c => c.Message).HasMaxLength(2000);
        builder.Property(c => c.DateCreation).IsRequired();

        builder.HasIndex(c => new { c.AnnonceEmploiId, c.CandidatId }).IsUnique();
    }
}
