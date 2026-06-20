using CVTech.BuildingBlocks.Exceptions;
using CVTech.BuildingBlocks.Permissions;
using CVTech.CatalogueEmploi.Application.Features.ConsulterCandidaturesRecues;
using CVTech.CatalogueEmploi.Domaine;
using FluentAssertions;
using Xunit;

namespace CVTech.CatalogueEmploi.Tests.Application;

public sealed class ConsulterCandidaturesRecuesHandlerTests
{
    [Fact]
    public async Task UneEntrepriseProprietairePeutConsulterLesCandidaturesDeSonAnnonce()
    {
        using var dbContext = OutilsTest.CreerContexte();
        var entrepriseId = Guid.NewGuid();

        var annonce = AnnonceEmploi.Publier("Annonce", "Description", TypeContrat.Cdi, "dev-web", "Paris", entrepriseId);
        dbContext.Annonces.Add(annonce);
        dbContext.Candidatures.Add(Candidature.Soumettre(annonce.Id, Guid.NewGuid(), "Motivé"));
        await dbContext.SaveChangesAsync();

        var handler = new ConsulterCandidaturesRecuesHandler(dbContext, OutilsTest.CreerVerificateurPermission());

        var resultat = await handler.Handle(new ConsulterCandidaturesRecuesQuery(RoleUtilisateur.Entreprise, entrepriseId, annonce.Id), CancellationToken.None);

        resultat.Should().ContainSingle();
    }

    [Fact]
    public async Task UneEntrepriseNonProprietaireNePeutPasConsulterLesCandidaturesDuneAutreAnnonce()
    {
        using var dbContext = OutilsTest.CreerContexte();

        var annonce = AnnonceEmploi.Publier("Annonce", "Description", TypeContrat.Cdi, "dev-web", "Paris", Guid.NewGuid());
        dbContext.Annonces.Add(annonce);
        await dbContext.SaveChangesAsync();

        var handler = new ConsulterCandidaturesRecuesHandler(dbContext, OutilsTest.CreerVerificateurPermission());

        var action = () => handler.Handle(new ConsulterCandidaturesRecuesQuery(RoleUtilisateur.Entreprise, Guid.NewGuid(), annonce.Id), CancellationToken.None);

        await action.Should().ThrowAsync<PermissionRefuseeException>();
    }

    [Fact]
    public async Task UnAdministrateurPeutConsulterLesCandidaturesDeNimporteQuelleAnnonce()
    {
        using var dbContext = OutilsTest.CreerContexte();

        var annonce = AnnonceEmploi.Publier("Annonce", "Description", TypeContrat.Cdi, "dev-web", "Paris", Guid.NewGuid());
        dbContext.Annonces.Add(annonce);
        dbContext.Candidatures.Add(Candidature.Soumettre(annonce.Id, Guid.NewGuid(), null));
        await dbContext.SaveChangesAsync();

        var handler = new ConsulterCandidaturesRecuesHandler(dbContext, OutilsTest.CreerVerificateurPermission());

        var resultat = await handler.Handle(new ConsulterCandidaturesRecuesQuery(RoleUtilisateur.Administrateur, Guid.NewGuid(), annonce.Id), CancellationToken.None);

        resultat.Should().ContainSingle();
    }
}
