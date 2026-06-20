using Xunit;
using CVTech.AppelOffreFreelance.Application.Features.SelectionnerLaureat;
using CVTech.AppelOffreFreelance.Domaine;
using CVTech.AppelOffreFreelance.Domaine.Exceptions;
using CVTech.BuildingBlocks.Exceptions;
using CVTech.BuildingBlocks.Permissions;
using FluentAssertions;

namespace CVTech.AppelOffreFreelance.Tests.Application;

public sealed class SelectionnerLaureatHandlerTests
{
    [Fact]
    public async Task UneEntrepriseProprietairePeutSelectionnerLeLaureat()
    {
        var ctx = OutilsTest.CreerContexte();
        var entrepriseId = Guid.NewGuid();
        var ao = AppelOffre.Publier("Mission", "Desc", "dev-web", "Paris", 500m, entrepriseId);
        var prop = PropositionFreelance.Soumettre(ao.Id, Guid.NewGuid(), "Prop", 400m, 20);
        ctx.AppelsOffres.Add(ao);
        ctx.Propositions.Add(prop);
        await ctx.SaveChangesAsync();

        var handler = new SelectionnerLaureatHandler(ctx, OutilsTest.CreerVerificateurPermission());
        await handler.Handle(new SelectionnerLaureatCommand(RoleUtilisateur.Entreprise, entrepriseId, ao.Id, prop.Id), CancellationToken.None);

        var appelOffreMaj = await ctx.AppelsOffres.FindAsync(ao.Id);
        appelOffreMaj!.LaureatId.Should().Be(prop.Id);
    }

    [Fact]
    public async Task OnNePeutPasSelectionnerDeuxFoisUnLaureat()
    {
        var ctx = OutilsTest.CreerContexte();
        var entrepriseId = Guid.NewGuid();
        var ao = AppelOffre.Publier("Mission", "Desc", "dev-web", "Paris", 500m, entrepriseId);
        var prop1 = PropositionFreelance.Soumettre(ao.Id, Guid.NewGuid(), "Prop 1", 400m, 20);
        var prop2 = PropositionFreelance.Soumettre(ao.Id, Guid.NewGuid(), "Prop 2", 350m, 25);
        ctx.AppelsOffres.Add(ao);
        ctx.Propositions.AddRange(prop1, prop2);
        await ctx.SaveChangesAsync();

        var handler = new SelectionnerLaureatHandler(ctx, OutilsTest.CreerVerificateurPermission());
        await handler.Handle(new SelectionnerLaureatCommand(RoleUtilisateur.Entreprise, entrepriseId, ao.Id, prop1.Id), CancellationToken.None);

        await Assert.ThrowsAsync<LaureatDejaSelectionneException>(() =>
            handler.Handle(new SelectionnerLaureatCommand(RoleUtilisateur.Entreprise, entrepriseId, ao.Id, prop2.Id), CancellationToken.None));
    }

    [Fact]
    public async Task UneEntrepriseNonProprietaireNePeutPasSelectionnerLeLaureat()
    {
        var ctx = OutilsTest.CreerContexte();
        var ao = AppelOffre.Publier("Mission", "Desc", "dev-web", "Paris", 500m, Guid.NewGuid());
        var prop = PropositionFreelance.Soumettre(ao.Id, Guid.NewGuid(), "Prop", 400m, 20);
        ctx.AppelsOffres.Add(ao);
        ctx.Propositions.Add(prop);
        await ctx.SaveChangesAsync();

        var handler = new SelectionnerLaureatHandler(ctx, OutilsTest.CreerVerificateurPermission());
        var autreEntrepriseId = Guid.NewGuid();

        await Assert.ThrowsAsync<PermissionRefuseeException>(() =>
            handler.Handle(new SelectionnerLaureatCommand(RoleUtilisateur.Entreprise, autreEntrepriseId, ao.Id, prop.Id), CancellationToken.None));
    }
}
