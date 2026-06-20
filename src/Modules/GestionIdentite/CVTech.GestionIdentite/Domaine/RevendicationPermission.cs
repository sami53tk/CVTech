using CVTech.BuildingBlocks.Permissions;

namespace CVTech.GestionIdentite.Domaine;

/// <summary>
/// Associe un rôle à une action métier qu'il est autorisé à effectuer. La <see cref="MatricePermissions"/>
/// est la collection de toutes les revendications de la plateforme.
/// </summary>
public sealed record RevendicationPermission(RoleUtilisateur Role, string Action);
