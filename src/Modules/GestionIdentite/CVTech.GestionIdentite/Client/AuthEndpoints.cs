using CVTech.GestionIdentite.Application.Features.InscrireUtilisateur;
using CVTech.GestionIdentite.Application.Features.SeConnecter;
using CVTech.GestionIdentite.Client.Dto;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CVTech.GestionIdentite.Client;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var groupe = app.MapGroup("/api/auth").WithTags("Authentification");

        groupe.MapPost("/inscription", async (InscriptionRequete requete, ISender sender, CancellationToken ct) =>
        {
            var resultat = await sender.Send(
                new InscrireUtilisateurCommand(requete.Email, requete.MotDePasse, requete.Role, requete.Nom, requete.Prenom, requete.RaisonSociale),
                ct);

            return Results.Created(
                $"/api/profils/{resultat.UtilisateurId}",
                new JetonReponse(resultat.UtilisateurId, resultat.Role.ToString(), resultat.Jeton));
        });

        groupe.MapPost("/connexion", async (ConnexionRequete requete, ISender sender, CancellationToken ct) =>
        {
            var resultat = await sender.Send(new SeConnecterCommand(requete.Email, requete.MotDePasse), ct);

            return Results.Ok(new JetonReponse(resultat.UtilisateurId, resultat.Role.ToString(), resultat.Jeton));
        });

        return app;
    }
}
