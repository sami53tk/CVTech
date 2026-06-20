using CVTech.BuildingBlocks.Exceptions;

namespace CVTech.BuildingBlocks.Permissions;

/// <summary>
/// Contrat public exposé par le module GestionIdentite (seul module à connaître la matrice
/// des rôles) et consommé par les autres modules via le ModuleLoader. Aucune action métier
/// ne doit s'exécuter sans être passée par ce contrat.
/// </summary>
public interface IVerificateurPermission
{
    /// <summary>
    /// Indique si le rôle donné est autorisé à effectuer l'action donnée, selon la matrice
    /// de permissions. Ne tient pas compte de l'état du compte (bloqué/actif) ni de la
    /// propriété de la ressource (« ses annonces », « ses appels d'offres ») : ces vérifications
    /// complémentaires restent à la charge du handler de cas d'usage.
    /// </summary>
    bool ARoleLaPermission(RoleUtilisateur role, string action);

    /// <summary>
    /// Vérifie que le rôle donné est autorisé à effectuer l'action donnée et lève
    /// <see cref="PermissionRefuseeException"/> dans le cas contraire.
    /// </summary>
    /// <exception cref="PermissionRefuseeException">Le rôle ne possède pas cette permission.</exception>
    void VerifierPermission(RoleUtilisateur role, string action);

    /// <summary>
    /// Vérifie que le compte de l'utilisateur donné est actif (non bloqué par un administrateur).
    /// </summary>
    /// <exception cref="CompteBloqueException">Le compte a été bloqué.</exception>
    Task VerifierCompteActifAsync(Guid utilisateurId, CancellationToken ct = default);
}
