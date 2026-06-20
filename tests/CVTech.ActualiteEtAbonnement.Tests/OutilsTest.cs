using CVTech.ActualiteEtAbonnement.Infrastructure;
using CVTech.BuildingBlocks.Exceptions;
using CVTech.BuildingBlocks.Permissions;
using Microsoft.EntityFrameworkCore;

namespace CVTech.ActualiteEtAbonnement.Tests;

internal static class OutilsTest
{
    public static ActualiteEtAbonnementDbContext CreerContexte() =>
        new(new DbContextOptionsBuilder<ActualiteEtAbonnementDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);

    public static IVerificateurPermission CreerVerificateurPermission() => new FauxVerificateurPermission();
}

internal sealed class FauxVerificateurPermission : IVerificateurPermission
{
    public void VerifierPermission(RoleUtilisateur role, string permission)
    {
        var autorise = (role, permission) switch
        {
            (RoleUtilisateur.Candidat, Permissions.SAbonnerDomaineMetier) => true,
            (RoleUtilisateur.Entreprise, Permissions.SAbonnerDomaineMetier) => true,
            (RoleUtilisateur.Administrateur, _) => true,
            _ => false,
        };
        if (!autorise) throw new PermissionRefuseeException(permission);
    }

    public bool ARoleLaPermission(RoleUtilisateur role, string action)
    {
        return (role, action) switch
        {
            (RoleUtilisateur.Candidat, Permissions.SAbonnerDomaineMetier) => true,
            (RoleUtilisateur.Entreprise, Permissions.SAbonnerDomaineMetier) => true,
            (RoleUtilisateur.Administrateur, _) => true,
            _ => false,
        };
    }

    public Task VerifierCompteActifAsync(Guid utilisateurId, CancellationToken ct = default)
        => Task.CompletedTask;
}
