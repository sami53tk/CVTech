using System.Security.Claims;
using CVTech.BuildingBlocks.Securite;
using CVTech.GestionIdentite.Application.Features.BloquerCompte;
using CVTech.GestionIdentite.Application.Features.ReactiverCompte;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CVTech.GestionIdentite.Client;

public static class ComptesEndpoints
{
    public static IEndpointRouteBuilder MapComptesEndpoints(this IEndpointRouteBuilder app)
    {
        var groupe = app.MapGroup("/api/comptes").WithTags("Comptes").RequireAuthorization();

        groupe.MapPost("/{id:guid}/bloquer", async (Guid id, ClaimsPrincipal utilisateur, ISender sender, CancellationToken ct) =>
        {
            await sender.Send(new BloquerCompteCommand(utilisateur.ObtenirRole(), id), ct);
            return Results.NoContent();
        });

        groupe.MapPost("/{id:guid}/reactiver", async (Guid id, ClaimsPrincipal utilisateur, ISender sender, CancellationToken ct) =>
        {
            await sender.Send(new ReactiverCompteCommand(utilisateur.ObtenirRole(), id), ct);
            return Results.NoContent();
        });

        return app;
    }
}
