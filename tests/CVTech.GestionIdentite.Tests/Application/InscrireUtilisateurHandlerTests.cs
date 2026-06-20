using CVTech.BuildingBlocks.Permissions;
using CVTech.GestionIdentite.Application.Features.InscrireUtilisateur;
using CVTech.GestionIdentite.Domaine.Exceptions;
using FluentAssertions;
using Xunit;

namespace CVTech.GestionIdentite.Tests.Application;

public sealed class InscrireUtilisateurHandlerTests
{
    [Fact]
    public async Task UnCandidatPeutSinscrireEtRecoitUnJeton()
    {
        using var dbContext = OutilsTest.CreerContexte();
        var handler = new InscrireUtilisateurHandler(dbContext, OutilsTest.CreerHacheurMotDePasse(), OutilsTest.CreerGenerateurJeton());

        var resultat = await handler.Handle(
            new InscrireUtilisateurCommand("ada@example.com", "MotDePasse123!", RoleUtilisateur.Candidat, "Lovelace", "Ada", null),
            CancellationToken.None);

        resultat.UtilisateurId.Should().NotBe(Guid.Empty);
        resultat.Role.Should().Be(RoleUtilisateur.Candidat);
        resultat.Jeton.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task DeuxUtilisateursNePeuventPasPartagerLaMemeAdresseEmail()
    {
        using var dbContext = OutilsTest.CreerContexte();
        var handler = new InscrireUtilisateurHandler(dbContext, OutilsTest.CreerHacheurMotDePasse(), OutilsTest.CreerGenerateurJeton());

        await handler.Handle(
            new InscrireUtilisateurCommand("ada@example.com", "MotDePasse123!", RoleUtilisateur.Candidat, "Lovelace", "Ada", null),
            CancellationToken.None);

        var action = () => handler.Handle(
            new InscrireUtilisateurCommand("ada@example.com", "AutreMotDePasse123!", RoleUtilisateur.Entreprise, null, null, "Autre Société"),
            CancellationToken.None);

        await action.Should().ThrowAsync<EmailDejaUtiliseException>();
    }
}
