using Xunit;
using CVTech.AppelOffreFreelance.Application.Features.SoumettreProposition;
using CVTech.AppelOffreFreelance.Domaine;
using CVTech.AppelOffreFreelance.Domaine.Exceptions;
using CVTech.BuildingBlocks.Exceptions;
using CVTech.BuildingBlocks.Permissions;
using FluentAssertions;

namespace CVTech.AppelOffreFreelance.Tests.Application;

public sealed class SoumettrePropositionHandlerTests
{
    private static async Task<(CVTech.AppelOffreFreelance.Infrastructure.AppelOffreFreelanceDbContext ctx, Guid appelOffreId)> CreerContexteAvecAppelOffre()
    {
        var ctx = OutilsTest.CreerContexte();
        var ao = AppelOffre.Publier("Mission", "Desc", "dev-web", "Paris", 500m, Guid.NewGuid());
        ctx.AppelsOffres.Add(ao);
        await ctx.SaveChangesAsync();
        return (ctx, ao.Id);
    }

    [Fact]
    public async Task UnCandidatPeutSoumettreUneProposition()
    {
        var (ctx, appelOffreId) = await CreerContexteAvecAppelOffre();
        var handler = new SoumettrePropositionHandler(ctx, OutilsTest.CreerVerificateurPermission());
        var cmd = new SoumettrePropositionCommand(RoleUtilisateur.Candidat, Guid.NewGuid(),
            appelOffreId, "Ma proposition", 450m, 30);


        var resultat = await handler.Handle(cmd, CancellationToken.None);

        resultat.PropositionId.Should().NotBeEmpty();
    }

    [Fact]
    public async Task UnCandidatNePeutPasSoumettreDeuxPropositionsSurLeMemeAppelOffre()
    {
        var (ctx, appelOffreId) = await CreerContexteAvecAppelOffre();
        var handler = new SoumettrePropositionHandler(ctx, OutilsTest.CreerVerificateurPermission());
        var freelanceId = Guid.NewGuid();
        var cmd = new SoumettrePropositionCommand(RoleUtilisateur.Candidat, freelanceId, appelOffreId, "Prop 1", 450m, 30);

        await handler.Handle(cmd, CancellationToken.None);
        await Assert.ThrowsAsync<PropositionDejaSubmiseException>(() =>
            handler.Handle(cmd with { Description = "Prop 2" }, CancellationToken.None));
    }

    [Fact]
    public async Task UneEntrepriseNePeutPasSoumettreUneProposition()
    {
        var (ctx, appelOffreId) = await CreerContexteAvecAppelOffre();
        var handler = new SoumettrePropositionHandler(ctx, OutilsTest.CreerVerificateurPermission());
        var cmd = new SoumettrePropositionCommand(RoleUtilisateur.Entreprise, Guid.NewGuid(),
            appelOffreId, "Prop", 500m, 10);

        await Assert.ThrowsAsync<PermissionRefuseeException>(() =>
            handler.Handle(cmd, CancellationToken.None));
    }

    [Fact]
    public async Task OnNePeutPasSoumettreUnePropositionSurUnAppelOffreInexistant()
    {
        var ctx = OutilsTest.CreerContexte();
        var handler = new SoumettrePropositionHandler(ctx, OutilsTest.CreerVerificateurPermission());
        var cmd = new SoumettrePropositionCommand(RoleUtilisateur.Candidat, Guid.NewGuid(),
            Guid.NewGuid(), "Prop", 400m, 20);

        await Assert.ThrowsAsync<EntiteIntrouvableException>(() =>
            handler.Handle(cmd, CancellationToken.None));
    }
}
