using MediatR;

namespace CVTech.ActualiteEtAbonnement.Application.Features.ConsulterNotifications;

public sealed record ConsulterNotificationsQuery(Guid UtilisateurId) : IRequest<IReadOnlyCollection<NotificationResume>>;

public sealed record NotificationResume(
    Guid Id,
    string Message,
    string DomaineMetierCode,
    string TypeSource,
    Guid SourceId,
    bool EstLue,
    DateTimeOffset DateCreation);
