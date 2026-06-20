using CVTech.BuildingBlocks.Exceptions;
using CVTech.BuildingBlocks.Permissions;
using CVTech.CatalogueEmploi.Application.Features.PostulerAnnonce;
using CVTech.CatalogueEmploi.Domaine;
using CVTech.CatalogueEmploi.Domaine.Exceptions;
using FluentAssertions;
using Xunit;

namespace CVTech.CatalogueEmploi.Tests.Application;

public sealed class PostulerAnnonceHandlerTests
{
    [Fact]
    public async Task UnCandidatAvecUnCvPeutPostulerAUneAnnonce()
    {
        using var dbContext = OutilsTest.CreerContexte();
        var candidatId = Guid.NewGuid();

        var annonce = AnnonceEmploi.Publier("Développeur .NET", "Description", TypeContrat.Cdi, "dev-web", "Paris", Guid.NewGuid());
        dbContext.Annonces.Add(annonce);
        dbContext.CurriculumsVitae.Add(CurriculumVitae.Constituer(candidatId, "Mon CV", "Résumé", ["C#", ".NET"]));
        await dbContext.SaveChangesAsync();

        var handler = new PostulerAnnonceHandler(dbContext, OutilsTest.CreerVerificateurPermission());

        var resultat = await handler.Handle(new PostulerAnnonceCommand(RoleUtilisateur.Candidat, candidatId, annonce.Id, "Motivé"), CancellationToken.None);

        resultat.CandidatureId.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public async Task UnCandidatSansCvNePeutPasPostulerAUneAnnonce()
    {
        using var dbContext = OutilsTest.CreerContexte();

        var annonce = AnnonceEmploi.Publier("Développeur .NET", "Description", TypeContrat.Cdi, "dev-web", "Paris", Guid.NewGuid());
        dbContext.Annonces.Add(annonce);
        await dbContext.SaveChangesAsync();

        var handler = new PostulerAnnonceHandler(dbContext, OutilsTest.CreerVerificateurPermission());

        var action = () => handler.Handle(new PostulerAnnonceCommand(RoleUtilisateur.Candidat, Guid.NewGuid(), annonce.Id, null), CancellationToken.None);

        await action.Should().ThrowAsync<CvObligatoirePourPostulerException>();
    }

    [Fact]
    public async Task UnCandidatNePeutPasPostulerDeuxFoisALaMemeAnnonce()
    {
        using var dbContext = OutilsTest.CreerContexte();
        var candidatId = Guid.NewGuid();

        var annonce = AnnonceEmploi.Publier("Développeur .NET", "Description", TypeContrat.Cdi, "dev-web", "Paris", Guid.NewGuid());
        dbContext.Annonces.Add(annonce);
        dbContext.CurriculumsVitae.Add(CurriculumVitae.Constituer(candidatId, "Mon CV", "Résumé", ["C#"]));
        await dbContext.SaveChangesAsync();

        var handler = new PostulerAnnonceHandler(dbContext, OutilsTest.CreerVerificateurPermission());

        await handler.Handle(new PostulerAnnonceCommand(RoleUtilisateur.Candidat, candidatId, annonce.Id, null), CancellationToken.None);

        var action = () => handler.Handle(new PostulerAnnonceCommand(RoleUtilisateur.Candidat, candidatId, annonce.Id, null), CancellationToken.None);

        await action.Should().ThrowAsync<CandidatureDejaSoumiseException>();
    }

    [Fact]
    public async Task UneEntrepriseNePeutPasPostulerAUneAnnonce()
    {
        using var dbContext = OutilsTest.CreerContexte();

        var annonce = AnnonceEmploi.Publier("Développeur .NET", "Description", TypeContrat.Cdi, "dev-web", "Paris", Guid.NewGuid());
        dbContext.Annonces.Add(annonce);
        await dbContext.SaveChangesAsync();

        var handler = new PostulerAnnonceHandler(dbContext, OutilsTest.CreerVerificateurPermission());

        var action = () => handler.Handle(new PostulerAnnonceCommand(RoleUtilisateur.Entreprise, Guid.NewGuid(), annonce.Id, null), CancellationToken.None);

        await action.Should().ThrowAsync<PermissionRefuseeException>();
    }
}
