using CVTech.ActualiteEtAbonnement.Application.Features.PublierArticle;
using CVTech.ActualiteEtAbonnement.Client;
using CVTech.ActualiteEtAbonnement.Domaine;
using CVTech.ActualiteEtAbonnement.Infrastructure;
using CVTech.ActualiteEtAbonnement.Infrastructure.GestionnaireEvenements;
using CVTech.BuildingBlocks.Comportements;
using CVTech.BuildingBlocks.Domaine;
using CVTech.BuildingBlocks.Evenements;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CVTech.ActualiteEtAbonnement;

public static class ModuleLoader
{
    private static readonly (string Code, string Libelle)[] DomainesParDefaut =
    [
        ("dev-web", "Développement Web"),
        ("cloud-azure", "Cloud & Azure"),
        ("data-science", "Data Science"),
        ("cybersecurite", "Cybersécurité"),
        ("devops", "DevOps"),
    ];

    public static IServiceCollection AjouterModuleActualiteEtAbonnement(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ActualiteEtAbonnementDbContext>(options =>
            options.UseSqlite(
                configuration.GetConnectionString("ActualiteEtAbonnement")
                ?? "Data Source=data/actualite-abonnement.db"));

        services.AddScoped<IReferentielDomaineMetier, ReferentielDomaineMetier>();

        // Gestionnaires d'événements du bus in-memory
        services.AddSingleton<IGestionnaireEvenement<AnnoncePublieeEvent>, GestionnaireAnnoncePubliee>();
        services.AddSingleton<IGestionnaireEvenement<AppelOffrePublieEvent>, GestionnaireAppelOffrePublie>();

        services.AddValidatorsFromAssembly(typeof(PublierArticleHandler).Assembly);

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(PublierArticleHandler).Assembly);
            cfg.AddOpenBehavior(typeof(ComportementValidation<,>));
        });

        return services;
    }

    public static IEndpointRouteBuilder MapperEndpointsActualiteEtAbonnement(this IEndpointRouteBuilder app)
    {
        app.MapActualiteEndpoints();
        return app;
    }

    public static async Task MigrerActualiteEtAbonnementAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ActualiteEtAbonnementDbContext>();
        await dbContext.Database.MigrateAsync();
    }

    public static async Task SeedActualiteEtAbonnementAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ActualiteEtAbonnementDbContext>();

        if (!await dbContext.DomainesMetier.AnyAsync())
        {
            foreach (var (code, libelle) in DomainesParDefaut)
            {
                dbContext.DomainesMetier.Add(DomaineMetierReference.Creer(code, libelle));
            }
            await dbContext.SaveChangesAsync();
        }
    }
}
