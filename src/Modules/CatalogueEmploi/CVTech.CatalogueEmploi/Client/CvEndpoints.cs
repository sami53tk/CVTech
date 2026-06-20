using System.Security.Claims;
using CVTech.BuildingBlocks.Securite;
using CVTech.CatalogueEmploi.Application.Features.ConstituerCv;
using CVTech.CatalogueEmploi.Application.Features.ConsulterMonCv;
using CVTech.CatalogueEmploi.Client.Dto;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CVTech.CatalogueEmploi.Client;

public static class CvEndpoints
{
    public static IEndpointRouteBuilder MapCvEndpoints(this IEndpointRouteBuilder app)
    {
        var groupe = app.MapGroup("/api/cv").WithTags("CV").RequireAuthorization();

        groupe.MapGet("/", async (ClaimsPrincipal utilisateur, ISender sender, CancellationToken ct) =>
        {
            var resultat = await sender.Send(new ConsulterMonCvQuery(utilisateur.ObtenirUtilisateurId()), ct);
            return resultat is null ? Results.NoContent() : Results.Ok(resultat);
        });

        groupe.MapPut("/", async (CvRequete requete, ClaimsPrincipal utilisateur, ISender sender, CancellationToken ct) =>
        {
            var resultat = await sender.Send(
                new ConstituerCvCommand(utilisateur.ObtenirRole(), utilisateur.ObtenirUtilisateurId(), requete.Titre, requete.Resume, requete.Competences),
                ct);

            return Results.Ok(resultat);
        });

        return app;
    }
}
