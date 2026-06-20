using CVTech.ActualiteEtAbonnement.Domaine;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CVTech.ActualiteEtAbonnement.Infrastructure.Configurations;

public sealed class ArticleActualiteConfiguration : IEntityTypeConfiguration<ArticleActualite>
{
    public void Configure(EntityTypeBuilder<ArticleActualite> builder)
    {
        builder.ToTable("Articles");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Titre).HasMaxLength(256).IsRequired();
        builder.Property(a => a.Contenu).IsRequired();
        builder.Property(a => a.DomaineMetierCode).HasMaxLength(64);
        builder.Property(a => a.LienExterne).HasMaxLength(512).IsRequired();
        builder.Property(a => a.AuteurId).IsRequired();
        builder.Property(a => a.DateCreation).IsRequired();
    }
}
