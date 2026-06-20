using System.Text.Json;
using CVTech.CatalogueEmploi.Domaine;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CVTech.CatalogueEmploi.Infrastructure.Configurations;

public sealed class CurriculumVitaeConfiguration : IEntityTypeConfiguration<CurriculumVitae>
{
    public void Configure(EntityTypeBuilder<CurriculumVitae> builder)
    {
        builder.ToTable("CurriculumsVitae");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.CandidatId).IsRequired();
        builder.HasIndex(c => c.CandidatId).IsUnique();

        builder.Property(c => c.Titre).HasMaxLength(256).IsRequired();
        builder.Property(c => c.Resume).IsRequired();

        builder.Property(c => c.Competences)
            .HasConversion(
                competences => JsonSerializer.Serialize(competences, (JsonSerializerOptions?)null),
                json => JsonSerializer.Deserialize<List<string>>(json, (JsonSerializerOptions?)null) ?? new List<string>())
            .Metadata.SetValueComparer(new Microsoft.EntityFrameworkCore.ChangeTracking.ValueComparer<List<string>>(
                (a, b) => (a ?? new List<string>()).SequenceEqual(b ?? new List<string>()),
                v => v.Aggregate(0, (hash, item) => HashCode.Combine(hash, item.GetHashCode())),
                v => v.ToList()));

        builder.Property(c => c.DateDerniereModification).IsRequired();
        builder.Property(c => c.DateCreation).IsRequired();
    }
}
