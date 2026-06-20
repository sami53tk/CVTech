using CVTech.CatalogueEmploi.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CVTech.CatalogueEmploi.Application.Features.ConsulterAnnonces;

public sealed class ConsulterAnnoncesHandler : IRequestHandler<ConsulterAnnoncesQuery, IReadOnlyCollection<AnnonceResume>>
{
    private readonly CatalogueEmploiDbContext _dbContext;

    public ConsulterAnnoncesHandler(CatalogueEmploiDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<AnnonceResume>> Handle(ConsulterAnnoncesQuery request, CancellationToken cancellationToken)
    {
        var requete = _dbContext.Annonces.Where(a => !a.EstSupprimee);

        if (!string.IsNullOrWhiteSpace(request.DomaineMetierCode))
        {
            requete = requete.Where(a => a.DomaineMetierCode == request.DomaineMetierCode);
        }

        if (request.TypeContrat is not null)
        {
            requete = requete.Where(a => a.TypeContrat == request.TypeContrat);
        }

        var annonces = await requete
            .Select(a => new AnnonceResume(a.Id, a.Titre, a.TypeContrat, a.DomaineMetierCode, a.Localisation, a.EntrepriseId, a.DateCreation))
            .ToListAsync(cancellationToken);

        // Tri en mémoire : SQLite ne sait pas trier par DateTimeOffset côté serveur.
        return annonces.OrderByDescending(a => a.DatePublication).ToList();
    }
}
