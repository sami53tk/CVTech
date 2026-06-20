using CVTech.CatalogueEmploi.Application.Features.ConsulterAnnonces;
using CVTech.CatalogueEmploi.Domaine;
using FluentAssertions;
using Xunit;

namespace CVTech.CatalogueEmploi.Tests.Application;

public sealed class ConsulterAnnoncesHandlerTests
{
    [Fact]
    public async Task LaListeDesAnnoncesNeContientPasLesAnnoncesModerees()
    {
        using var dbContext = OutilsTest.CreerContexte();

        var annonceVisible = AnnonceEmploi.Publier("Annonce visible", "Description", TypeContrat.Cdi, "dev-web", "Paris", Guid.NewGuid());
        var annonceModeree = AnnonceEmploi.Publier("Annonce modérée", "Description", TypeContrat.Cdi, "dev-web", "Lyon", Guid.NewGuid());
        annonceModeree.Moderer();

        dbContext.Annonces.AddRange(annonceVisible, annonceModeree);
        await dbContext.SaveChangesAsync();

        var handler = new ConsulterAnnoncesHandler(dbContext);

        var resultat = await handler.Handle(new ConsulterAnnoncesQuery(null, null), CancellationToken.None);

        resultat.Should().ContainSingle().Which.Id.Should().Be(annonceVisible.Id);
    }

    [Fact]
    public async Task LaListeDesAnnoncesPeutEtreFiltreeParDomaineMetier()
    {
        using var dbContext = OutilsTest.CreerContexte();

        var annonceDevWeb = AnnonceEmploi.Publier("Annonce dev web", "Description", TypeContrat.Cdi, "dev-web", "Paris", Guid.NewGuid());
        var annonceCloud = AnnonceEmploi.Publier("Annonce cloud", "Description", TypeContrat.Cdi, "cloud-azure", "Paris", Guid.NewGuid());

        dbContext.Annonces.AddRange(annonceDevWeb, annonceCloud);
        await dbContext.SaveChangesAsync();

        var handler = new ConsulterAnnoncesHandler(dbContext);

        var resultat = await handler.Handle(new ConsulterAnnoncesQuery("cloud-azure", null), CancellationToken.None);

        resultat.Should().ContainSingle().Which.Id.Should().Be(annonceCloud.Id);
    }
}
