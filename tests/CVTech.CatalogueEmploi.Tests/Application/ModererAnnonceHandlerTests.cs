using CVTech.BuildingBlocks.Exceptions;
using CVTech.BuildingBlocks.Permissions;
using CVTech.CatalogueEmploi.Application.Features.ModererAnnonce;
using CVTech.CatalogueEmploi.Domaine;
using FluentAssertions;
using Xunit;

namespace CVTech.CatalogueEmploi.Tests.Application;

public sealed class ModererAnnonceHandlerTests
{
    [Fact]
    public async Task UnAdministrateurPeutModererUneAnnonce()
    {
        using var dbContext = OutilsTest.CreerContexte();

        var annonce = AnnonceEmploi.Publier("Annonce litigieuse", "Description", TypeContrat.Cdi, "dev-web", "Paris", Guid.NewGuid());
        dbContext.Annonces.Add(annonce);
        await dbContext.SaveChangesAsync();

        var handler = new ModererAnnonceHandler(dbContext, OutilsTest.CreerVerificateurPermission());

        await handler.Handle(new ModererAnnonceCommand(RoleUtilisateur.Administrateur, annonce.Id), CancellationToken.None);

        annonce.EstSupprimee.Should().BeTrue();
    }

    [Fact]
    public async Task UneEntrepriseNePeutPasModererUneAnnonce()
    {
        using var dbContext = OutilsTest.CreerContexte();

        var annonce = AnnonceEmploi.Publier("Annonce litigieuse", "Description", TypeContrat.Cdi, "dev-web", "Paris", Guid.NewGuid());
        dbContext.Annonces.Add(annonce);
        await dbContext.SaveChangesAsync();

        var handler = new ModererAnnonceHandler(dbContext, OutilsTest.CreerVerificateurPermission());

        var action = () => handler.Handle(new ModererAnnonceCommand(RoleUtilisateur.Entreprise, annonce.Id), CancellationToken.None);

        await action.Should().ThrowAsync<PermissionRefuseeException>();
        annonce.EstSupprimee.Should().BeFalse();
    }
}
