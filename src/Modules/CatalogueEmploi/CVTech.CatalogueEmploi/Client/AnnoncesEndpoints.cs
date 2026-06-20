using System.Security.Claims;
using CVTech.BuildingBlocks.Securite;
using CVTech.CatalogueEmploi.Application.Features.ConsulterAnnonce;
using CVTech.CatalogueEmploi.Application.Features.ConsulterAnnonces;
using CVTech.CatalogueEmploi.Application.Features.ConsulterCandidaturesRecues;
using CVTech.CatalogueEmploi.Application.Features.ModererAnnonce;
using CVTech.CatalogueEmploi.Application.Features.PostulerAnnonce;
using CVTech.CatalogueEmploi.Application.Features.PublierAnnonce;
using CVTech.CatalogueEmploi.Client.Dto;
using CVTech.CatalogueEmploi.Domaine;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CVTech.CatalogueEmploi.Client;

public static class AnnoncesEndpoints
{
    public static IEndpointRouteBuilder MapAnnoncesEndpoints(this IEndpointRouteBuilder app)
    {
        var groupe = app.MapGroup("/api/annonces").WithTags("Annonces");

        groupe.MapGet("/", async (string? domaine, TypeContrat? typeContrat, ISender sender, CancellationToken ct) =>
        {
            var resultat = await sender.Send(new ConsulterAnnoncesQuery(domaine, typeContrat), ct);
            return Results.Ok(resultat);
        });

        groupe.MapGet("/{id:guid}", async (Guid id, ISender sender, CancellationToken ct) =>
        {
            var resultat = await sender.Send(new ConsulterAnnonceQuery(id), ct);
            return Results.Ok(resultat);
        });

        groupe.MapPost("/", async (PublierAnnonceRequete requete, ClaimsPrincipal utilisateur, ISender sender, CancellationToken ct) =>
        {
            var resultat = await sender.Send(
                new PublierAnnonceCommand(utilisateur.ObtenirRole(), utilisateur.ObtenirUtilisateurId(), requete.Titre, requete.Description, requete.TypeContrat, requete.DomaineMetierCode, requete.Localisation),
                ct);

            return Results.Created($"/api/annonces/{resultat.AnnonceId}", resultat);
        }).RequireAuthorization();

        groupe.MapDelete("/{id:guid}", async (Guid id, ClaimsPrincipal utilisateur, ISender sender, CancellationToken ct) =>
        {
            await sender.Send(new ModererAnnonceCommand(utilisateur.ObtenirRole(), id), ct);
            return Results.NoContent();
        }).RequireAuthorization();

        groupe.MapPost("/{id:guid}/candidatures", async (Guid id, CandidatureRequete requete, ClaimsPrincipal utilisateur, ISender sender, CancellationToken ct) =>
        {
            var resultat = await sender.Send(
                new PostulerAnnonceCommand(utilisateur.ObtenirRole(), utilisateur.ObtenirUtilisateurId(), id, requete.Message),
                ct);

            return Results.Created($"/api/annonces/{id}/candidatures/{resultat.CandidatureId}", resultat);
        }).RequireAuthorization();

        groupe.MapGet("/{id:guid}/candidatures", async (Guid id, ClaimsPrincipal utilisateur, ISender sender, CancellationToken ct) =>
        {
            var resultat = await sender.Send(
                new ConsulterCandidaturesRecuesQuery(utilisateur.ObtenirRole(), utilisateur.ObtenirUtilisateurId(), id),
                ct);

            return Results.Ok(resultat);
        }).RequireAuthorization();

        return app;
    }
}
