using System.Security.Claims;
using CVTech.BuildingBlocks.Permissions;

namespace CVTech.BuildingBlocks.Securite;

/// <summary>
/// Lecture des informations d'identité (utilisateur, rôle) portées par le jeton JWT émis par
/// GestionIdentite, telles qu'exposées au Client layer de chaque module via <see cref="ClaimsPrincipal"/>.
/// </summary>
public static class ClaimsPrincipalExtensions
{
    public static Guid ObtenirUtilisateurId(this ClaimsPrincipal utilisateur)
    {
        var valeur = utilisateur.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? throw new InvalidOperationException("Le jeton ne contient pas d'identifiant utilisateur.");

        return Guid.Parse(valeur);
    }

    public static RoleUtilisateur ObtenirRole(this ClaimsPrincipal utilisateur)
    {
        var valeur = utilisateur.FindFirst(ClaimTypes.Role)?.Value
            ?? throw new InvalidOperationException("Le jeton ne contient pas de rôle utilisateur.");

        return Enum.Parse<RoleUtilisateur>(valeur);
    }
}
