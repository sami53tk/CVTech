using CVTech.ActualiteEtAbonnement.Domaine;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CVTech.ActualiteEtAbonnement.Infrastructure.Configurations;

public sealed class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("Notifications");
        builder.HasKey(n => n.Id);
        builder.Property(n => n.UtilisateurId).IsRequired();
        builder.Property(n => n.Message).HasMaxLength(512).IsRequired();
        builder.Property(n => n.DomaineMetierCode).HasMaxLength(64).IsRequired();
        builder.Property(n => n.TypeSource).HasMaxLength(32).IsRequired();
        builder.Property(n => n.SourceId).IsRequired();
        builder.Property(n => n.EstLue).IsRequired();
        builder.Property(n => n.DateCreation).IsRequired();
    }
}
