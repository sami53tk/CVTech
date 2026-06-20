using System.Security.Claims;
using CVTech.BuildingBlocks.Securite;
using CVTech.GestionIdentite.Application.Features.ConsulterProfil;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CVTech.GestionIdentite.Client;

public static class ProfilsEndpoints
{
    public static IEndpointRouteBuilder MapProfilsEndpoints(this IEndpointRouteBuilder app)
    {
        var groupe = app.MapGroup("/api/profils").WithTags("Profils").RequireAuthorization();

        groupe.MapGet("/moi", async (ClaimsPrincipal utilisateur, ISender sender, CancellationToken ct) =>
        {
            var resultat = await sender.Send(new ConsulterProfilQuery(utilisateur.ObtenirUtilisateurId()), ct);
            return Results.Ok(resultat);
        });

        return app;
    }
}
