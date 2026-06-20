using Xunit;
using CVTech.AppelOffreFreelance.Application.Features.PublierAppelOffre;
using CVTech.BuildingBlocks.Evenements;
using CVTech.BuildingBlocks.Exceptions;
using CVTech.BuildingBlocks.Permissions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace CVTech.AppelOffreFreelance.Tests.Application;

public sealed class PublierAppelOffreHandlerTests
{
    [Fact]
    public async Task UneEntreprisePeutPublierUnAppelOffreEtLEvenementEstPublie()
    {
        var ctx = OutilsTest.CreerContexte();
        var bus = (BusEvenementsDeTest)OutilsTest.CreerBusEvenements();
        var handler = new PublierAppelOffreHandler(ctx, OutilsTest.CreerVerificateurPermission(), bus);
        var cmd = new PublierAppelOffreCommand(RoleUtilisateur.Entreprise, Guid.NewGuid(),
            "Mission DevOps", "Description mission", "dev-web", "Paris", 500m);

        var resultat = await handler.Handle(cmd, CancellationToken.None);

        resultat.AppelOffreId.Should().NotBeEmpty();
        var appelOffre = await ctx.AppelsOffres.FindAsync(resultat.AppelOffreId);
        appelOffre.Should().NotBeNull();
        appelOffre!.Titre.Should().Be("Mission DevOps");
        bus.EvenementsPublies.Should().HaveCount(1)
            .And.ContainItemsAssignableTo<AppelOffrePublieEvent>();
    }

    [Fact]
    public async Task UnCandidatNePeutPasPublierUnAppelOffre()
    {
        var handler = new PublierAppelOffreHandler(
            OutilsTest.CreerContexte(), OutilsTest.CreerVerificateurPermission(), OutilsTest.CreerBusEvenements());
        var cmd = new PublierAppelOffreCommand(RoleUtilisateur.Candidat, Guid.NewGuid(),
            "Mission", "Desc", "dev-web", "Paris", 500m);

        await Assert.ThrowsAsync<PermissionRefuseeException>(() =>
            handler.Handle(cmd, CancellationToken.None));
    }

    [Fact]
    public async Task OnNePeutPasPublierUnAppelOffreDansUnDomaineMetierInconnu()
    {
        var ctx = OutilsTest.CreerContexte();
        var validator = new PublierAppelOffreValidator(OutilsTest.CreerReferentielDomaineMetier());
        var cmd = new PublierAppelOffreCommand(RoleUtilisateur.Entreprise, Guid.NewGuid(),
            "Mission", "Desc", "domaine-inexistant", "Paris", 500m);

        var resultat = await validator.ValidateAsync(cmd);

        resultat.IsValid.Should().BeFalse();
        resultat.Errors.Should().Contain(e => e.PropertyName == "DomaineMetierCode");
    }
}
