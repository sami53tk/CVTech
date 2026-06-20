using CVTech.AppelOffreFreelance.Domaine;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CVTech.AppelOffreFreelance.Infrastructure.Configurations;

public sealed class PropositionFreelanceConfiguration : IEntityTypeConfiguration<PropositionFreelance>
{
    public void Configure(EntityTypeBuilder<PropositionFreelance> builder)
    {
        builder.ToTable("Propositions");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.AppelOffreId).IsRequired();
        builder.Property(p => p.FreelanceId).IsRequired();
        builder.Property(p => p.Description).IsRequired();
        builder.Property(p => p.TauxJournalier).HasColumnType("decimal(18,2)");
        builder.Property(p => p.DureeEstimeeJours).IsRequired();
        builder.Property(p => p.DateCreation).IsRequired();

        builder.HasIndex(p => new { p.AppelOffreId, p.FreelanceId }).IsUnique();
    }
}
