using CVTech.AppelOffreFreelance.Infrastructure;
using CVTech.BuildingBlocks.Domaine;
using CVTech.BuildingBlocks.Evenements;
using CVTech.BuildingBlocks.Exceptions;
using CVTech.BuildingBlocks.Permissions;
using Microsoft.EntityFrameworkCore;

namespace CVTech.AppelOffreFreelance.Tests;

internal static class OutilsTest
{
    public static AppelOffreFreelanceDbContext CreerContexte() =>
        new(new DbContextOptionsBuilder<AppelOffreFreelanceDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);

    public static IVerificateurPermission CreerVerificateurPermission() => new FauxVerificateurPermission();
    public static IReferentielDomaineMetier CreerReferentielDomaineMetier() => new FauxReferentielDomaineMetier();
    public static IBusEvenements CreerBusEvenements() => new BusEvenementsDeTest();
}

internal sealed class FauxVerificateurPermission : IVerificateurPermission
{
    public void VerifierPermission(RoleUtilisateur role, string permission)
    {
        var autorise = (role, permission) switch
        {
            (RoleUtilisateur.Candidat, Permissions.SoumettrePropositionFreelance) => true,
            (RoleUtilisateur.Entreprise, Permissions.PublierAppelOffre) => true,
            (RoleUtilisateur.Entreprise, Permissions.ConsulterPropositionsRecues) => true,
            (RoleUtilisateur.Entreprise, Permissions.SelectionnerLaureat) => true,
            (RoleUtilisateur.Administrateur, Permissions.PublierAppelOffre) => true,
            (RoleUtilisateur.Administrateur, Permissions.ConsulterPropositionsRecues) => true,
            (RoleUtilisateur.Administrateur, Permissions.ModererAnnonceOuAppelOffre) => true,
            (RoleUtilisateur.Administrateur, Permissions.SelectionnerLaureat) => true,
            _ => false,
        };
        if (!autorise) throw new PermissionRefuseeException(permission);
    }

    public bool ARoleLaPermission(RoleUtilisateur role, string action)
    {
        return (role, action) switch
        {
            (RoleUtilisateur.Candidat, Permissions.SoumettrePropositionFreelance) => true,
            (RoleUtilisateur.Entreprise, Permissions.PublierAppelOffre) => true,
            (RoleUtilisateur.Entreprise, Permissions.ConsulterPropositionsRecues) => true,
            (RoleUtilisateur.Entreprise, Permissions.SelectionnerLaureat) => true,
            (RoleUtilisateur.Administrateur, _) => true,
            _ => false,
        };
    }

    public Task VerifierCompteActifAsync(Guid utilisateurId, CancellationToken ct = default)
        => Task.CompletedTask;
}

internal sealed class FauxReferentielDomaineMetier : IReferentielDomaineMetier
{
    private static readonly string[] _codes = ["dev-web", "cloud-azure", "data-science"];

    public Task<IReadOnlyCollection<DomaineMetier>> ListerAsync(CancellationToken ct = default)
        => Task.FromResult<IReadOnlyCollection<DomaineMetier>>(
            _codes.Select(c => new DomaineMetier(c, c)).ToList());

    public Task<bool> ExisteAsync(string code, CancellationToken ct = default)
        => Task.FromResult(_codes.Contains(code.Trim().ToLowerInvariant()));
}

internal sealed class BusEvenementsDeTest : IBusEvenements
{
    public List<IEvenementIntegration> EvenementsPublies { get; } = new();

    public Task PublierAsync<T>(T evenement, CancellationToken ct = default) where T : IEvenementIntegration
    {
        EvenementsPublies.Add(evenement);
        return Task.CompletedTask;
    }
}
