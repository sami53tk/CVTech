using CVTech.ActualiteEtAbonnement.Domaine;
using CVTech.BuildingBlocks.Evenements;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CVTech.ActualiteEtAbonnement.Infrastructure.GestionnaireEvenements;

public sealed class GestionnaireAppelOffrePublie : IGestionnaireEvenement<AppelOffrePublieEvent>
{
    private readonly IServiceProvider _services;
    private readonly ILogger<GestionnaireAppelOffrePublie> _logger;

    public GestionnaireAppelOffrePublie(IServiceProvider services, ILogger<GestionnaireAppelOffrePublie> logger)
    {
        _services = services;
        _logger = logger;
    }

    public async Task GererAsync(AppelOffrePublieEvent evenement, CancellationToken ct = default)
    {
        using var scope = _services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ActualiteEtAbonnementDbContext>();

        var abonnes = await dbContext.Abonnements
            .Where(a => a.DomaineMetierCode == evenement.DomaineMetierCode)
            .Select(a => a.UtilisateurId)
            .ToListAsync(ct);

        foreach (var utilisateurId in abonnes)
        {
            var message = $"Nouvel appel d'offre freelance : « {evenement.Titre} » dans le domaine {evenement.DomaineMetierCode}.";
            var notification = Notification.Creer(utilisateurId, message, evenement.DomaineMetierCode, "AppelOffre", evenement.AppelOffreId);
            dbContext.Notifications.Add(notification);

            _logger.LogInformation("[EMAIL] → utilisateur {Id} : {Message}", utilisateurId, message);
        }

        if (abonnes.Count > 0)
            await dbContext.SaveChangesAsync(ct);
    }
}
