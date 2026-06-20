using CVTech.BuildingBlocks.Exceptions;
using CVTech.GestionIdentite.Application.Features.SeConnecter;
using CVTech.GestionIdentite.Domaine;
using CVTech.GestionIdentite.Domaine.Exceptions;
using CVTech.GestionIdentite.Domaine.ValueObjects;
using FluentAssertions;
using Xunit;

namespace CVTech.GestionIdentite.Tests.Application;

public sealed class SeConnecterHandlerTests
{
    [Fact]
    public async Task UnCandidatActifPeutSeConnecterAvecSesIdentifiants()
    {
        using var dbContext = OutilsTest.CreerContexte();
        var hacheur = OutilsTest.CreerHacheurMotDePasse();

        var candidat = ProfilCandidat.Inscrire(new Email("ada@example.com"), hacheur.Hacher("MotDePasse123!"), "Lovelace", "Ada");
        dbContext.Utilisateurs.Add(candidat);
        await dbContext.SaveChangesAsync();

        var handler = new SeConnecterHandler(dbContext, hacheur, OutilsTest.CreerGenerateurJeton());

        var resultat = await handler.Handle(new SeConnecterCommand("ada@example.com", "MotDePasse123!"), CancellationToken.None);

        resultat.UtilisateurId.Should().Be(candidat.Id);
        resultat.Jeton.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task UnMotDePasseIncorrectEmpecheLaConnexion()
    {
        using var dbContext = OutilsTest.CreerContexte();
        var hacheur = OutilsTest.CreerHacheurMotDePasse();

        var candidat = ProfilCandidat.Inscrire(new Email("ada@example.com"), hacheur.Hacher("MotDePasse123!"), "Lovelace", "Ada");
        dbContext.Utilisateurs.Add(candidat);
        await dbContext.SaveChangesAsync();

        var handler = new SeConnecterHandler(dbContext, hacheur, OutilsTest.CreerGenerateurJeton());

        var action = () => handler.Handle(new SeConnecterCommand("ada@example.com", "MauvaisMotDePasse"), CancellationToken.None);

        await action.Should().ThrowAsync<IdentifiantsInvalidesException>();
    }

    [Fact]
    public async Task UnCompteBloqueNePeutPasSeConnecter()
    {
        using var dbContext = OutilsTest.CreerContexte();
        var hacheur = OutilsTest.CreerHacheurMotDePasse();

        var candidat = ProfilCandidat.Inscrire(new Email("ada@example.com"), hacheur.Hacher("MotDePasse123!"), "Lovelace", "Ada");
        candidat.Bloquer();
        dbContext.Utilisateurs.Add(candidat);
        await dbContext.SaveChangesAsync();

        var handler = new SeConnecterHandler(dbContext, hacheur, OutilsTest.CreerGenerateurJeton());

        var action = () => handler.Handle(new SeConnecterCommand("ada@example.com", "MotDePasse123!"), CancellationToken.None);

        await action.Should().ThrowAsync<CompteBloqueException>();
    }
}
