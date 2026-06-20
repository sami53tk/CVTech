using MediatR;

namespace CVTech.ActualiteEtAbonnement.Application.Features.ConsulterFeedRss;

public sealed record ConsulterFeedRssQuery(string? DomaineMetierCode) : IRequest<FeedRssResultat>;

public sealed record ItemRss(
    Guid Id,
    string Titre,
    string Description,
    string Lien,
    DateTimeOffset DatePublication);

public sealed record FeedRssResultat(IReadOnlyCollection<ItemRss> Items);
