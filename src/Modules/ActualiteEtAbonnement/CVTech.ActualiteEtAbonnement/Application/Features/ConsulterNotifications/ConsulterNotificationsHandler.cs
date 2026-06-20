using CVTech.ActualiteEtAbonnement.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CVTech.ActualiteEtAbonnement.Application.Features.ConsulterNotifications;

public sealed class ConsulterNotificationsHandler : IRequestHandler<ConsulterNotificationsQuery, IReadOnlyCollection<NotificationResume>>
{
    private readonly ActualiteEtAbonnementDbContext _dbContext;

    public ConsulterNotificationsHandler(ActualiteEtAbonnementDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<NotificationResume>> Handle(ConsulterNotificationsQuery request, CancellationToken cancellationToken)
    {
        var notifications = await _dbContext.Notifications
            .Where(n => n.UtilisateurId == request.UtilisateurId)
            .Select(n => new NotificationResume(n.Id, n.Message, n.DomaineMetierCode, n.TypeSource, n.SourceId, n.EstLue, n.DateCreation))
            .ToListAsync(cancellationToken);

        // Tri en mémoire : SQLite ne sait pas trier par DateTimeOffset côté serveur.
        return notifications.OrderByDescending(n => n.DateCreation).ToList();
    }
}
