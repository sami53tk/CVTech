using CVTech.BuildingBlocks.Permissions;
using Permissions = CVTech.BuildingBlocks.Permissions.Permissions;

namespace CVTech.GestionIdentite.Domaine;

/// <summary>
/// Traduction en code de la « Matrice de Permissions » du README. C'est la seule source de
/// vérité pour savoir si un rôle peut effectuer une action métier donnée.
/// </summary>
public static class MatricePermissions
{
    public static readonly IReadOnlyCollection<RevendicationPermission> Revendications = new[]
    {
        // Candidat
        new RevendicationPermission(RoleUtilisateur.Candidat, Permissions.ConstituerCv),
        new RevendicationPermission(RoleUtilisateur.Candidat, Permissions.PostulerAnnonce),
        new RevendicationPermission(RoleUtilisateur.Candidat, Permissions.SoumettrePropositionFreelance),
        new RevendicationPermission(RoleUtilisateur.Candidat, Permissions.SAbonnerDomaineMetier),

        // Entreprise
        new RevendicationPermission(RoleUtilisateur.Entreprise, Permissions.PublierAnnonce),
        new RevendicationPermission(RoleUtilisateur.Entreprise, Permissions.PublierAppelOffre),
        new RevendicationPermission(RoleUtilisateur.Entreprise, Permissions.ConsulterCandidaturesRecues),
        new RevendicationPermission(RoleUtilisateur.Entreprise, Permissions.ConsulterPropositionsRecues),
        new RevendicationPermission(RoleUtilisateur.Entreprise, Permissions.SAbonnerDomaineMetier),

        // Administrateur (hérite des droits métier, hors actions strictement candidat
        // « postuler » et « soumettre une proposition », + droits de modération exclusifs)
        new RevendicationPermission(RoleUtilisateur.Administrateur, Permissions.ConstituerCv),
        new RevendicationPermission(RoleUtilisateur.Administrateur, Permissions.SAbonnerDomaineMetier),
        new RevendicationPermission(RoleUtilisateur.Administrateur, Permissions.PublierAnnonce),
        new RevendicationPermission(RoleUtilisateur.Administrateur, Permissions.PublierAppelOffre),
        new RevendicationPermission(RoleUtilisateur.Administrateur, Permissions.ConsulterCandidaturesRecues),
        new RevendicationPermission(RoleUtilisateur.Administrateur, Permissions.ConsulterPropositionsRecues),
        new RevendicationPermission(RoleUtilisateur.Administrateur, Permissions.PublierArticleActualite),
        new RevendicationPermission(RoleUtilisateur.Administrateur, Permissions.ModererAnnonceOuAppelOffre),
        new RevendicationPermission(RoleUtilisateur.Administrateur, Permissions.BloquerReactiverCompte),
        new RevendicationPermission(RoleUtilisateur.Administrateur, Permissions.GererReferentielDomaines),
    };

    public static bool ARoleLaPermission(RoleUtilisateur role, string action)
        => Revendications.Any(r => r.Role == role && r.Action == action);
}
