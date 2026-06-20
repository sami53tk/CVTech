using CVTech.ActualiteEtAbonnement.Application.Features.ConsulterFeedRss;
using CVTech.ActualiteEtAbonnement.Application.Features.ConsulterNotifications;
using CVTech.ActualiteEtAbonnement.Application.Features.GererAbonnement;
using CVTech.ActualiteEtAbonnement.Application.Features.GererDomaines;
using CVTech.ActualiteEtAbonnement.Application.Features.PublierArticle;
using CVTech.ActualiteEtAbonnement.Client.Dto;
using CVTech.BuildingBlocks.Securite;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Text;

namespace CVTech.ActualiteEtAbonnement.Client;

public static class ActualiteEndpoints
{
    public static IEndpointRouteBuilder MapActualiteEndpoints(this IEndpointRouteBuilder app)
    {
        // ---- Flux RSS 2.0 (public) ----
        app.MapGet("/feed/rss", async (IMediator mediator, string? domaine, CancellationToken ct) =>
        {
            var resultat = await mediator.Send(new ConsulterFeedRssQuery(domaine), ct);
            var xml = GenererRss(resultat);
            return Results.Content(xml, "application/rss+xml; charset=utf-8");
        });

        // ---- Articles (admin) ----
        app.MapPost("/api/articles", async (IMediator mediator, HttpContext ctx, PublierArticleRequete requete, CancellationToken ct) =>
        {
            var role = ctx.User.ObtenirRole();
            var auteurId = ctx.User.ObtenirUtilisateurId();
            var resultat = await mediator.Send(new PublierArticleCommand(
                role, auteurId, requete.Titre, requete.Contenu, requete.DomaineMetierCode, requete.LienExterne), ct);
            return Results.Created($"/api/articles/{resultat.ArticleId}", resultat);
        }).RequireAuthorization();

        // ---- Abonnements ----
        app.MapGet("/api/abonnements", async (IMediator mediator, HttpContext ctx, CancellationToken ct) =>
        {
            var utilisateurId = ctx.User.ObtenirUtilisateurId();
            return Results.Ok(await mediator.Send(new ConsulterAbonnementsQuery(utilisateurId), ct));
        }).RequireAuthorization();

        app.MapPost("/api/abonnements", async (IMediator mediator, HttpContext ctx, SAbonnerRequete requete, CancellationToken ct) =>
        {
            var role = ctx.User.ObtenirRole();
            var utilisateurId = ctx.User.ObtenirUtilisateurId();
            var id = await mediator.Send(new SAbonnerCommand(role, utilisateurId, requete.DomaineMetierCode), ct);
            return Results.Created($"/api/abonnements/{id}", new { id });
        }).RequireAuthorization();

        app.MapDelete("/api/abonnements/{id:guid}", async (IMediator mediator, HttpContext ctx, Guid id, CancellationToken ct) =>
        {
            var role = ctx.User.ObtenirRole();
            var utilisateurId = ctx.User.ObtenirUtilisateurId();
            await mediator.Send(new SeDesabonnerCommand(role, utilisateurId, id), ct);
            return Results.NoContent();
        }).RequireAuthorization();

        // ---- Notifications ----
        app.MapGet("/api/notifications", async (IMediator mediator, HttpContext ctx, CancellationToken ct) =>
        {
            var utilisateurId = ctx.User.ObtenirUtilisateurId();
            return Results.Ok(await mediator.Send(new ConsulterNotificationsQuery(utilisateurId), ct));
        }).RequireAuthorization();

        // ---- Référentiel domaines (admin) ----
        app.MapGet("/api/domaines-metier", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new ListerDomainesQuery(), ct)));

        app.MapPost("/api/domaines-metier", async (IMediator mediator, HttpContext ctx, CreerDomaineRequete requete, CancellationToken ct) =>
        {
            var role = ctx.User.ObtenirRole();
            var id = await mediator.Send(new CreerDomaineCommand(role, requete.Code, requete.Libelle), ct);
            return Results.Created($"/api/domaines-metier/{id}", new { id });
        }).RequireAuthorization();

        app.MapDelete("/api/domaines-metier/{id:guid}", async (IMediator mediator, HttpContext ctx, Guid id, CancellationToken ct) =>
        {
            var role = ctx.User.ObtenirRole();
            await mediator.Send(new SupprimerDomaineCommand(role, id), ct);
            return Results.NoContent();
        }).RequireAuthorization();

        return app;
    }

    private static string GenererRss(FeedRssResultat resultat)
    {
        var sb = new StringBuilder();
        sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        sb.AppendLine("<rss version=\"2.0\">");
        sb.AppendLine("  <channel>");
        sb.AppendLine("    <title>CVTech — Actualités</title>");
        sb.AppendLine("    <link>http://localhost:5298/feed/rss</link>");
        sb.AppendLine("    <description>Le fil éditorial de la plateforme CVTech.</description>");
        sb.AppendLine("    <language>fr-FR</language>");

        foreach (var item in resultat.Items)
        {
            sb.AppendLine("    <item>");
            sb.AppendLine($"      <title>{Escape(item.Titre)}</title>");
            sb.AppendLine($"      <link>{Escape(item.Lien)}</link>");
            sb.AppendLine($"      <description>{Escape(item.Description)}</description>");
            sb.AppendLine($"      <pubDate>{item.DatePublication:R}</pubDate>");
            sb.AppendLine($"      <guid>{item.Id}</guid>");
            sb.AppendLine("    </item>");
        }

        sb.AppendLine("  </channel>");
        sb.AppendLine("</rss>");
        return sb.ToString();
    }

    private static string Escape(string s) =>
        s.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
}
