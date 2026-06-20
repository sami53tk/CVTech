using CVTech.BuildingBlocks.Exceptions;
using CVTech.BuildingBlocks.Permissions;
using CVTech.GestionIdentite.Domaine;
using Microsoft.EntityFrameworkCore;

namespace CVTech.GestionIdentite.Infrastructure;

/// <summary>
/// Implémentation de référence d'<see cref="IVerificateurPermission"/>, enregistrée par le
/// ModuleLoader de GestionIdentite et consommée par les trois autres modules via DI.
/// </summary>
public sealed class VerificateurPermission : IVerificateurPermission
{
    private readonly IdentiteDbContext _dbContext;

    public VerificateurPermission(IdentiteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool ARoleLaPermission(RoleUtilisateur role, string action)
        => MatricePermissions.ARoleLaPermission(role, action);

    public void VerifierPermission(RoleUtilisateur role, string action)
    {
        if (!ARoleLaPermission(role, action))
        {
            throw new PermissionRefuseeException(action);
        }
    }

    public async Task VerifierCompteActifAsync(Guid utilisateurId, CancellationToken ct = default)
    {
        var utilisateur = await _dbContext.Utilisateurs
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == utilisateurId, ct);

        if (utilisateur is { EstBloque: true })
        {
            throw new CompteBloqueException();
        }
    }
}
