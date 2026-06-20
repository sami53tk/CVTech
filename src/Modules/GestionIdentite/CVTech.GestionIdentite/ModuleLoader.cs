using CVTech.BuildingBlocks.Comportements;
using CVTech.BuildingBlocks.Permissions;
using CVTech.GestionIdentite.Client;
using CVTech.GestionIdentite.Domaine;
using CVTech.GestionIdentite.Domaine.ValueObjects;
using CVTech.GestionIdentite.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Routing;

namespace CVTech.GestionIdentite;

/// <summary>
/// Composition root du module GestionIdentite : seul point d'entrée pour l'enregistrement de
/// ses services et de ses routes dans l'hôte API.
/// </summary>
public static class ModuleLoader
{
    public static IServiceCollection AjouterModuleGestionIdentite(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IdentiteDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("GestionIdentite") ?? "Data Source=data/identite.db"));

        services.Configure<OptionsJwt>(configuration.GetSection(OptionsJwt.Section));

        services.AddScoped<IHacheurMotDePasse, HacheurMotDePassePbkdf2>();
        services.AddScoped<IGenerateurJeton, GenerateurJetonJwt>();
        services.AddScoped<IVerificateurPermission, VerificateurPermission>();

        services.AddValidatorsFromAssembly(typeof(ModuleLoader).Assembly);

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ModuleLoader).Assembly);
            cfg.AddOpenBehavior(typeof(ComportementValidation<,>));
        });

        return services;
    }

    public static IEndpointRouteBuilder MapperEndpointsGestionIdentite(this IEndpointRouteBuilder app)
    {
        app.MapAuthEndpoints();
        app.MapProfilsEndpoints();
        app.MapComptesEndpoints();

        return app;
    }

    public static async Task MigrerGestionIdentiteAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IdentiteDbContext>();
        await dbContext.Database.MigrateAsync();
    }

    /// <summary>
    /// Crée le compte administrateur par défaut (admin@cvtech.fr / Admin123!) s'il n'existe pas
    /// encore, afin que la plateforme dispose toujours d'au moins un compte de chaque rôle.
    /// </summary>
    public static async Task SeedGestionIdentiteAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IdentiteDbContext>();
        var hacheurMotDePasse = scope.ServiceProvider.GetRequiredService<IHacheurMotDePasse>();

        var administrateurExiste = await dbContext.Utilisateurs
            .AnyAsync(u => u.Role == RoleUtilisateur.Administrateur);

        if (!administrateurExiste)
        {
            var admin = Administrateur.Creer(
                new Email("admin@cvtech.fr"),
                hacheurMotDePasse.Hacher("Admin123!"),
                "Administrateur Plateforme");

            dbContext.Utilisateurs.Add(admin);
            await dbContext.SaveChangesAsync();
        }
    }
}
