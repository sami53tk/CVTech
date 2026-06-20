using CVTech.AppelOffreFreelance.Domaine;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CVTech.AppelOffreFreelance.Infrastructure.Configurations;

public sealed class AppelOffreConfiguration : IEntityTypeConfiguration<AppelOffre>
{
    public void Configure(EntityTypeBuilder<AppelOffre> builder)
    {
        builder.ToTable("AppelsOffres");
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Titre).HasMaxLength(256).IsRequired();
        builder.Property(a => a.Description).IsRequired();
        builder.Property(a => a.DomaineMetierCode).HasMaxLength(64).IsRequired();
        builder.Property(a => a.Localisation).HasMaxLength(128).IsRequired();
        builder.Property(a => a.BudgetMax).HasColumnType("decimal(18,2)");
        builder.Property(a => a.EntrepriseId).IsRequired();
        builder.Property(a => a.EstSupprime).IsRequired();
        builder.Property(a => a.DateCreation).IsRequired();
    }
}
