using CVTech.BuildingBlocks.Exceptions;
using CVTech.BuildingBlocks.Permissions;
using CVTech.CatalogueEmploi.Application.Features.ConstituerCv;
using FluentAssertions;
using Xunit;

namespace CVTech.CatalogueEmploi.Tests.Application;

public sealed class ConstituerCvHandlerTests
{
    [Fact]
    public async Task UnCandidatPeutConstituerSonCv()
    {
        using var dbContext = OutilsTest.CreerContexte();
        var candidatId = Guid.NewGuid();

        var handler = new ConstituerCvHandler(dbContext, OutilsTest.CreerVerificateurPermission());

        var resultat = await handler.Handle(
            new ConstituerCvCommand(RoleUtilisateur.Candidat, candidatId, "Développeur full-stack", "5 ans d'expérience", ["C#", "React"]),
            CancellationToken.None);

        resultat.CvId.Should().NotBe(Guid.Empty);

        var cv = await dbContext.CurriculumsVitae.FindAsync(resultat.CvId);
        cv!.CandidatId.Should().Be(candidatId);
        cv.Competences.Should().Contain("React");
    }

    [Fact]
    public async Task UnCandidatPeutModifierSonCvExistant()
    {
        using var dbContext = OutilsTest.CreerContexte();
        var candidatId = Guid.NewGuid();
        var handler = new ConstituerCvHandler(dbContext, OutilsTest.CreerVerificateurPermission());

        await handler.Handle(new ConstituerCvCommand(RoleUtilisateur.Candidat, candidatId, "Titre initial", "Résumé initial", ["C#"]), CancellationToken.None);
        var resultat = await handler.Handle(new ConstituerCvCommand(RoleUtilisateur.Candidat, candidatId, "Titre modifié", "Résumé modifié", ["C#", "Azure"]), CancellationToken.None);

        var cvs = dbContext.CurriculumsVitae.Where(c => c.CandidatId == candidatId).ToList();
        cvs.Should().ContainSingle();
        cvs[0].Titre.Should().Be("Titre modifié");
        cvs[0].Id.Should().Be(resultat.CvId);
    }

    [Fact]
    public async Task UneEntrepriseNePeutPasConstituerUnCv()
    {
        using var dbContext = OutilsTest.CreerContexte();
        var handler = new ConstituerCvHandler(dbContext, OutilsTest.CreerVerificateurPermission());

        var action = () => handler.Handle(
            new ConstituerCvCommand(RoleUtilisateur.Entreprise, Guid.NewGuid(), "Titre", "Résumé", []),
            CancellationToken.None);

        await action.Should().ThrowAsync<PermissionRefuseeException>();
    }
}
