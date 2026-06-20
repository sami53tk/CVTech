using CVTech.BuildingBlocks.Comportements;
using CVTech.CatalogueEmploi.Client;
using CVTech.CatalogueEmploi.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CVTech.CatalogueEmploi;

/// <summary>
/// Composition root du module CatalogueEmploi : seul point d'entrée pour l'enregistrement de
/// ses services et de ses routes dans l'hôte API.
/// </summary>
public static class ModuleLoader
{
    public static IServiceCollection AjouterModuleCatalogueEmploi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CatalogueEmploiDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("CatalogueEmploi") ?? "Data Source=data/catalogue-emploi.db"));

        services.AddValidatorsFromAssembly(typeof(ModuleLoader).Assembly);

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ModuleLoader).Assembly);
            cfg.AddOpenBehavior(typeof(ComportementValidation<,>));
        });

        return services;
    }

    public static IEndpointRouteBuilder MapperEndpointsCatalogueEmploi(this IEndpointRouteBuilder app)
    {
        app.MapAnnoncesEndpoints();
        app.MapCvEndpoints();

        return app;
    }

    public static async Task MigrerCatalogueEmploiAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CatalogueEmploiDbContext>();
        await dbContext.Database.MigrateAsync();
    }
}
