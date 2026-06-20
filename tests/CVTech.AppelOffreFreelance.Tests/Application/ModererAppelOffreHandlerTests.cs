using Xunit;
using CVTech.AppelOffreFreelance.Application.Features.ModererAppelOffre;
using CVTech.AppelOffreFreelance.Domaine;
using CVTech.BuildingBlocks.Exceptions;
using CVTech.BuildingBlocks.Permissions;
using FluentAssertions;

namespace CVTech.AppelOffreFreelance.Tests.Application;

public sealed class ModererAppelOffreHandlerTests
{
    [Fact]
    public async Task UnAdministrateurPeutModererUnAppelOffre()
    {
        var ctx = OutilsTest.CreerContexte();
        var ao = AppelOffre.Publier("Mission", "Desc", "dev-web", "Paris", 500m, Guid.NewGuid());
        ctx.AppelsOffres.Add(ao);
        await ctx.SaveChangesAsync();

        var handler = new ModererAppelOffreHandler(ctx, OutilsTest.CreerVerificateurPermission());
        await handler.Handle(new ModererAppelOffreCommand(RoleUtilisateur.Administrateur, ao.Id), CancellationToken.None);

        var maj = await ctx.AppelsOffres.FindAsync(ao.Id);
        maj!.EstSupprime.Should().BeTrue();
    }

    [Fact]
    public async Task UneEntrepriseNePeutPasModererUnAppelOffre()
    {
        var ctx = OutilsTest.CreerContexte();
        var ao = AppelOffre.Publier("Mission", "Desc", "dev-web", "Paris", 500m, Guid.NewGuid());
        ctx.AppelsOffres.Add(ao);
        await ctx.SaveChangesAsync();

        var handler = new ModererAppelOffreHandler(ctx, OutilsTest.CreerVerificateurPermission());

        await Assert.ThrowsAsync<PermissionRefuseeException>(() =>
            handler.Handle(new ModererAppelOffreCommand(RoleUtilisateur.Entreprise, ao.Id), CancellationToken.None));
    }
}
