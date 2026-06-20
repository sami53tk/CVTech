using Xunit;
using CVTech.ActualiteEtAbonnement.Application.Features.GererAbonnement;
using CVTech.ActualiteEtAbonnement.Domaine;
using CVTech.ActualiteEtAbonnement.Infrastructure.GestionnaireEvenements;
using CVTech.BuildingBlocks.Evenements;
using CVTech.BuildingBlocks.Exceptions;
using CVTech.BuildingBlocks.Permissions;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;

namespace CVTech.ActualiteEtAbonnement.Tests.Application;

public sealed class AbonnementEtNotificationTests
{
    [Fact]
    public async Task UnCandidatPeutSAbonnerAUnDomaineMetier()
    {
        var ctx = OutilsTest.CreerContexte();
        var handler = new SAbonnerHandler(ctx, OutilsTest.CreerVerificateurPermission());
        var utilisateurId = Guid.NewGuid();

        var id = await handler.Handle(new SAbonnerCommand(RoleUtilisateur.Candidat, utilisateurId, "dev-web"), CancellationToken.None);

        id.Should().NotBeEmpty();
        ctx.Abonnements.Should().ContainSingle(a => a.UtilisateurId == utilisateurId && a.DomaineMetierCode == "dev-web");
    }

    [Fact]
    public async Task SAbonnerDeuxFoisRetourneLeMemeAbonnement()
    {
        var ctx = OutilsTest.CreerContexte();
        var handler = new SAbonnerHandler(ctx, OutilsTest.CreerVerificateurPermission());
        var utilisateurId = Guid.NewGuid();

        var id1 = await handler.Handle(new SAbonnerCommand(RoleUtilisateur.Candidat, utilisateurId, "dev-web"), CancellationToken.None);
        var id2 = await handler.Handle(new SAbonnerCommand(RoleUtilisateur.Candidat, utilisateurId, "dev-web"), CancellationToken.None);

        id1.Should().Be(id2);
        ctx.Abonnements.Count(a => a.UtilisateurId == utilisateurId).Should().Be(1);
    }

    [Fact]
    public async Task UnAbonneRecoitUneNotificationQuandUneAnnonceDeSonDomaineEstPubliee()
    {
        var ctx = OutilsTest.CreerContexte();
        var utilisateurId = Guid.NewGuid();

        // Abonnement préalable au domaine
        ctx.Abonnements.Add(Abonnement.Creer(utilisateurId, "dev-web"));
        await ctx.SaveChangesAsync();

        // Simuler le service provider pour le gestionnaire singleton
        var services = new ServiceCollection();
        services.AddSingleton(ctx);
        services.AddLogging();
        var serviceProvider = services.BuildServiceProvider();

        var gestionnaire = new GestionnaireAnnoncePubliee(serviceProvider, NullLogger<GestionnaireAnnoncePubliee>.Instance);
        var evenement = new AnnoncePublieeEvent(Guid.NewGuid(), "Développeur .NET", "dev-web", Guid.NewGuid(), DateTimeOffset.UtcNow);

        await gestionnaire.GererAsync(evenement);

        ctx.Notifications.Should().ContainSingle(n =>
            n.UtilisateurId == utilisateurId &&
            n.DomaineMetierCode == "dev-web" &&
            n.TypeSource == "AnnonceEmploi");
    }

    [Fact]
    public async Task UnUtilisateurNonAbonneNeRecoitPasDeNotification()
    {
        var ctx = OutilsTest.CreerContexte();
        // Aucun abonnement

        var services = new ServiceCollection();
        services.AddSingleton(ctx);
        services.AddLogging();
        var serviceProvider = services.BuildServiceProvider();

        var gestionnaire = new GestionnaireAnnoncePubliee(serviceProvider, NullLogger<GestionnaireAnnoncePubliee>.Instance);
        var evenement = new AnnoncePublieeEvent(Guid.NewGuid(), "Développeur Cloud", "cloud-azure", Guid.NewGuid(), DateTimeOffset.UtcNow);

        await gestionnaire.GererAsync(evenement);

        ctx.Notifications.Should().BeEmpty();
    }
}
