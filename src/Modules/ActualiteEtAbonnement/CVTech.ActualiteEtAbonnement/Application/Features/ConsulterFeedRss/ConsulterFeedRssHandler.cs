using CVTech.ActualiteEtAbonnement.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CVTech.ActualiteEtAbonnement.Application.Features.ConsulterFeedRss;

public sealed class ConsulterFeedRssHandler : IRequestHandler<ConsulterFeedRssQuery, FeedRssResultat>
{
    private readonly ActualiteEtAbonnementDbContext _dbContext;

    public ConsulterFeedRssHandler(ActualiteEtAbonnementDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<FeedRssResultat> Handle(ConsulterFeedRssQuery request, CancellationToken cancellationToken)
    {
        var requete = _dbContext.Articles.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.DomaineMetierCode))
        {
            requete = requete.Where(a => a.DomaineMetierCode == request.DomaineMetierCode);
        }

        // Seulement les articles éditoriaux (jamais les annonces/AO qui arrivent via événements).
        var articles = await requete
            .Select(a => new ItemRss(a.Id, a.Titre, a.Contenu, a.LienExterne, a.DateCreation))
            .ToListAsync(cancellationToken);

        // Tri en mémoire : SQLite ne sait pas trier par DateTimeOffset côté serveur.
        var itemsTries = articles.OrderByDescending(a => a.DatePublication).ToList();

        return new FeedRssResultat(itemsTries);
    }
}
