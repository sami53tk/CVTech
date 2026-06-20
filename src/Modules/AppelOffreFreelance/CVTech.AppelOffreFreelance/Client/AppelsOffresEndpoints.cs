using CVTech.AppelOffreFreelance.Application.Features.ConsulterAppelOffre;
using CVTech.AppelOffreFreelance.Application.Features.ConsulterAppelsOffres;
using CVTech.AppelOffreFreelance.Application.Features.ConsulterPropositionsRecues;
using CVTech.AppelOffreFreelance.Application.Features.ModererAppelOffre;
using CVTech.AppelOffreFreelance.Application.Features.PublierAppelOffre;
using CVTech.AppelOffreFreelance.Application.Features.SelectionnerLaureat;
using CVTech.AppelOffreFreelance.Application.Features.SoumettreProposition;
using CVTech.AppelOffreFreelance.Client.Dto;
using CVTech.BuildingBlocks.Securite;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CVTech.AppelOffreFreelance.Client;

public static class AppelsOffresEndpoints
{
    public static IEndpointRouteBuilder MapAppelsOffresEndpoints(this IEndpointRouteBuilder app)
    {
        var groupe = app.MapGroup("/api/appels-offres");

        // Public
        groupe.MapGet("/", async (IMediator mediator, string? domaine, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new ConsulterAppelsOffresQuery(domaine), ct)));

        groupe.MapGet("/{id:guid}", async (IMediator mediator, Guid id, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new ConsulterAppelOffreQuery(id), ct)));

        // Entreprise — publier un appel d'offre
        groupe.MapPost("/", async (IMediator mediator, HttpContext ctx, PublierAppelOffreRequete requete, CancellationToken ct) =>
        {
            var role = ctx.User.ObtenirRole();
            var entrepriseId = ctx.User.ObtenirUtilisateurId();
            var resultat = await mediator.Send(new PublierAppelOffreCommand(
                role, entrepriseId, requete.Titre, requete.Description,
                requete.DomaineMetierCode, requete.Localisation, requete.BudgetMax), ct);
            return Results.Created($"/api/appels-offres/{resultat.AppelOffreId}", resultat);
        }).RequireAuthorization();

        // Admin — modérer (supprimer) un appel d'offre
        groupe.MapDelete("/{id:guid}", async (IMediator mediator, HttpContext ctx, Guid id, CancellationToken ct) =>
        {
            var role = ctx.User.ObtenirRole();
            await mediator.Send(new ModererAppelOffreCommand(role, id), ct);
            return Results.NoContent();
        }).RequireAuthorization();

        // Candidat/Freelance — soumettre une proposition
        groupe.MapPost("/{id:guid}/propositions", async (IMediator mediator, HttpContext ctx,
            Guid id, SoumettrePropositionRequete requete, CancellationToken ct) =>
        {
            var role = ctx.User.ObtenirRole();
            var freelanceId = ctx.User.ObtenirUtilisateurId();
            var resultat = await mediator.Send(new SoumettrePropositionCommand(
                role, freelanceId, id, requete.Description, requete.TauxJournalier, requete.DureeEstimeeJours), ct);
            return Results.Created($"/api/appels-offres/{id}/propositions/{resultat.PropositionId}", resultat);
        }).RequireAuthorization();

        // Entreprise/Admin — consulter les propositions reçues
        groupe.MapGet("/{id:guid}/propositions", async (IMediator mediator, HttpContext ctx, Guid id, CancellationToken ct) =>
        {
            var role = ctx.User.ObtenirRole();
            var entrepriseId = ctx.User.ObtenirUtilisateurId();
            return Results.Ok(await mediator.Send(new ConsulterPropositionsRecuesQuery(role, entrepriseId, id), ct));
        }).RequireAuthorization();

        // Entreprise — sélectionner le lauréat
        groupe.MapPost("/{id:guid}/laureat", async (IMediator mediator, HttpContext ctx,
            Guid id, SelectionnerLaureatRequete requete, CancellationToken ct) =>
        {
            var role = ctx.User.ObtenirRole();
            var entrepriseId = ctx.User.ObtenirUtilisateurId();
            await mediator.Send(new SelectionnerLaureatCommand(role, entrepriseId, id, requete.PropositionId), ct);
            return Results.NoContent();
        }).RequireAuthorization();

        return app;
    }
}
