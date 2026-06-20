using CVTech.BuildingBlocks.Exceptions;
using CVTech.BuildingBlocks.Permissions;
using CVTech.GestionIdentite.Application.Features.BloquerCompte;
using CVTech.GestionIdentite.Domaine;
using CVTech.GestionIdentite.Domaine.ValueObjects;
using CVTech.GestionIdentite.Infrastructure;
using FluentAssertions;
using Xunit;

namespace CVTech.GestionIdentite.Tests.Application;

public sealed class BloquerCompteHandlerTests
{
    [Fact]
    public async Task UnAdministrateurPeutBloquerUnCompte()
    {
        using var dbContext = OutilsTest.CreerContexte();
        var hacheur = OutilsTest.CreerHacheurMotDePasse();

        var candidat = ProfilCandidat.Inscrire(new Email("ada@example.com"), hacheur.Hacher("MotDePasse123!"), "Lovelace", "Ada");
        dbContext.Utilisateurs.Add(candidat);
        await dbContext.SaveChangesAsync();

        var handler = new BloquerCompteHandler(dbContext, new VerificateurPermission(dbContext));

        await handler.Handle(new BloquerCompteCommand(RoleUtilisateur.Administrateur, candidat.Id), CancellationToken.None);

        candidat.EstBloque.Should().BeTrue();
    }

    [Fact]
    public async Task UnCandidatNePeutPasBloquerUnCompte()
    {
        using var dbContext = OutilsTest.CreerContexte();
        var hacheur = OutilsTest.CreerHacheurMotDePasse();

        var cible = ProfilCandidat.Inscrire(new Email("ada@example.com"), hacheur.Hacher("MotDePasse123!"), "Lovelace", "Ada");
        dbContext.Utilisateurs.Add(cible);
        await dbContext.SaveChangesAsync();

        var handler = new BloquerCompteHandler(dbContext, new VerificateurPermission(dbContext));

        var action = () => handler.Handle(new BloquerCompteCommand(RoleUtilisateur.Candidat, cible.Id), CancellationToken.None);

        await action.Should().ThrowAsync<PermissionRefuseeException>();
        cible.EstBloque.Should().BeFalse();
    }
}
