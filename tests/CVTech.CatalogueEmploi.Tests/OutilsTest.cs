using CVTech.BuildingBlocks.Domaine;
using CVTech.BuildingBlocks.Exceptions;
using CVTech.BuildingBlocks.Permissions;
using CVTech.CatalogueEmploi.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CVTech.CatalogueEmploi.Tests;

internal static class OutilsTest
{
    public static CatalogueEmploiDbContext CreerContexte()
    {
        var options = new DbContextOptionsBuilder<CatalogueEmploiDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new CatalogueEmploiDbContext(options);
    }

    public static IVerificateurPermission CreerVerificateurPermission() => new FauxVerificateurPermission();

    public static IReferentielDomaineMetier CreerReferentielDomaineMetier() => new FauxReferentielDomaineMetier();
}

/// <summary>
/// Double de test de <see cref="IVerificateurPermission"/> reproduisant, pour les besoins du
/// module CatalogueEmploi, le sous-ensemble pertinent de la matrice de permissions définie par
/// le module GestionIdentite.
/// </summary>
internal sealed class FauxVerificateurPermission : IVerificateurPermission
{
    public bool ARoleLaPermission(RoleUtilisateur role, string action) => (role, action) switch
    {
        (RoleUtilisateur.Candidat, Permissions.ConstituerCv) => true,
        (RoleUtilisateur.Candidat, Permissions.PostulerAnnonce) => true,
        (RoleUtilisateur.Entreprise, Permissions.PublierAnnonce) => true,
        (RoleUtilisateur.Entreprise, Permissions.ConsulterCandidaturesRecues) => true,
        (RoleUtilisateur.Administrateur, Permissions.ConstituerCv) => true,
        (RoleUtilisateur.Administrateur, Permissions.PublierAnnonce) => true,
        (RoleUtilisateur.Administrateur, Permissions.ConsulterCandidaturesRecues) => true,
        (RoleUtilisateur.Administrateur, Permissions.ModererAnnonceOuAppelOffre) => true,
        _ => false,
    };

    public void VerifierPermission(RoleUtilisateur role, string action)
    {
        if (!ARoleLaPermission(role, action))
        {
            throw new PermissionRefuseeException(action);
        }
    }

    public Task VerifierCompteActifAsync(Guid utilisateurId, CancellationToken ct = default) => Task.CompletedTask;
}

/// <summary>
/// Double de test de <see cref="IReferentielDomaineMetier"/> exposant un référentiel fixe de
/// domaines métier valides.
/// </summary>
internal sealed class FauxReferentielDomaineMetier : IReferentielDomaineMetier
{
    private static readonly IReadOnlyCollection<DomaineMetier> Domaines = new[]
    {
        new DomaineMetier("cloud-azure", "Cloud & Azure"),
        new DomaineMetier("dev-web", "Développement Web"),
        new DomaineMetier("data-science", "Data Science"),
    };

    public Task<IReadOnlyCollection<DomaineMetier>> ListerAsync(CancellationToken ct = default) => Task.FromResult(Domaines);

    public Task<bool> ExisteAsync(string code, CancellationToken ct = default) =>
        Task.FromResult(Domaines.Any(d => d.Code == code.Trim().ToLowerInvariant()));
}
