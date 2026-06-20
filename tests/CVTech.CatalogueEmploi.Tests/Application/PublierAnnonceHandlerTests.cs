using CVTech.BuildingBlocks.Evenements;
using CVTech.BuildingBlocks.Exceptions;
using CVTech.BuildingBlocks.Permissions;
using CVTech.CatalogueEmploi.Application.Features.PublierAnnonce;
using CVTech.CatalogueEmploi.Domaine;
using FluentAssertions;
using FluentValidation;
using Xunit;

namespace CVTech.CatalogueEmploi.Tests.Application;

public sealed class PublierAnnonceHandlerTests
{
    [Fact]
    public async Task UneEntreprisePeutPublierUneAnnonceEtLEvenementEstPublie()
    {
        using var dbContext = OutilsTest.CreerContexte();
        var busEvenements = new BusEvenementsDeTest();

        var handler = new PublierAnnonceHandler(dbContext, OutilsTest.CreerVerificateurPermission(), busEvenements);
        var entrepriseId = Guid.NewGuid();

        var resultat = await handler.Handle(
            new PublierAnnonceCommand(RoleUtilisateur.Entreprise, entrepriseId, "Développeur .NET", "Description du poste", TypeContrat.Cdi, "dev-web", "Paris"),
            CancellationToken.None);

        resultat.AnnonceId.Should().NotBe(Guid.Empty);

        var evenement = busEvenements.EvenementsPublies.Should().ContainSingle().Subject;
        var annoncePubliee = evenement.Should().BeOfType<AnnoncePublieeEvent>().Subject;
        annoncePubliee.AnnonceId.Should().Be(resultat.AnnonceId);
        annoncePubliee.DomaineMetierCode.Should().Be("dev-web");
        annoncePubliee.EntrepriseId.Should().Be(entrepriseId);
    }

    [Fact]
    public async Task UnCandidatNePeutPasPublierUneAnnonce()
    {
        using var dbContext = OutilsTest.CreerContexte();
        var handler = new PublierAnnonceHandler(dbContext, OutilsTest.CreerVerificateurPermission(), new BusEvenementsDeTest());

        var action = () => handler.Handle(
            new PublierAnnonceCommand(RoleUtilisateur.Candidat, Guid.NewGuid(), "Titre", "Description", TypeContrat.Cdi, "dev-web", "Paris"),
            CancellationToken.None);

        await action.Should().ThrowAsync<PermissionRefuseeException>();
    }

    [Fact]
    public async Task OnNePeutPasPublierUneAnnonceDansUnDomaineMetierInconnu()
    {
        var validator = new PublierAnnonceValidator(OutilsTest.CreerReferentielDomaineMetier());

        var resultat = await validator.ValidateAsync(
            new PublierAnnonceCommand(RoleUtilisateur.Entreprise, Guid.NewGuid(), "Titre", "Description", TypeContrat.Cdi, "domaine-inexistant", "Paris"));

        resultat.IsValid.Should().BeFalse();
    }
}

internal sealed class BusEvenementsDeTest : IBusEvenements
{
    public List<IEvenementIntegration> EvenementsPublies { get; } = [];

    public Task PublierAsync<TEvenement>(TEvenement evenement, CancellationToken ct = default) where TEvenement : IEvenementIntegration
    {
        EvenementsPublies.Add(evenement);
        return Task.CompletedTask;
    }
}
