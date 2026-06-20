using CVTech.AppelOffreFreelance.Application.Features.PublierAppelOffre;
using CVTech.AppelOffreFreelance.Client;
using CVTech.AppelOffreFreelance.Infrastructure;
using CVTech.BuildingBlocks.Comportements;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CVTech.AppelOffreFreelance;

public static class ModuleLoader
{
    public static IServiceCollection AjouterModuleAppelOffreFreelance(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppelOffreFreelanceDbContext>(options =>
            options.UseSqlite(
                configuration.GetConnectionString("AppelOffreFreelance")
                ?? "Data Source=data/appel-offre-freelance.db"));

        services.AddValidatorsFromAssembly(typeof(PublierAppelOffreValidator).Assembly);

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(PublierAppelOffreValidator).Assembly);
            cfg.AddOpenBehavior(typeof(ComportementValidation<,>));
        });

        return services;
    }

    public static IEndpointRouteBuilder MapperEndpointsAppelOffreFreelance(this IEndpointRouteBuilder app)
    {
        app.MapAppelsOffresEndpoints();
        return app;
    }

    public static async Task MigrerAppelOffreFreelanceAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppelOffreFreelanceDbContext>();
        await dbContext.Database.MigrateAsync();
    }
}
