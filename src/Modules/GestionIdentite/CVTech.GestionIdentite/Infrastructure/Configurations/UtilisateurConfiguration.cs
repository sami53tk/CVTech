using CVTech.GestionIdentite.Domaine;
using CVTech.GestionIdentite.Domaine.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CVTech.GestionIdentite.Infrastructure.Configurations;

public sealed class UtilisateurConfiguration : IEntityTypeConfiguration<Utilisateur>
{
    public void Configure(EntityTypeBuilder<Utilisateur> builder)
    {
        builder.ToTable("Utilisateurs");
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Email)
            .HasConversion(e => e.Valeur, v => new Email(v))
            .HasColumnName("Email")
            .HasMaxLength(256)
            .IsRequired();

        builder.HasIndex(u => u.Email).IsUnique();

        builder.Property(u => u.MotDePasse)
            .HasConversion(m => m.Valeur, v => new MotDePasseHache(v))
            .HasColumnName("MotDePasseHache")
            .IsRequired();

        builder.Property(u => u.Role)
            .HasConversion<string>()
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(u => u.EstBloque).IsRequired();
        builder.Property(u => u.DateCreation).IsRequired();

        builder.HasDiscriminator<string>("TypeUtilisateur")
            .HasValue<ProfilCandidat>("Candidat")
            .HasValue<ProfilEntreprise>("Entreprise")
            .HasValue<Administrateur>("Administrateur");
    }
}

public sealed class ProfilCandidatConfiguration : IEntityTypeConfiguration<ProfilCandidat>
{
    public void Configure(EntityTypeBuilder<ProfilCandidat> builder)
    {
        builder.Property(p => p.Nom).HasMaxLength(128);
        builder.Property(p => p.Prenom).HasMaxLength(128);
    }
}

public sealed class ProfilEntrepriseConfiguration : IEntityTypeConfiguration<ProfilEntreprise>
{
    public void Configure(EntityTypeBuilder<ProfilEntreprise> builder)
    {
        builder.Property(p => p.RaisonSociale).HasMaxLength(256);
    }
}

public sealed class AdministrateurConfiguration : IEntityTypeConfiguration<Administrateur>
{
    public void Configure(EntityTypeBuilder<Administrateur> builder)
    {
        builder.Property(a => a.Nom).HasMaxLength(128);
    }
}
