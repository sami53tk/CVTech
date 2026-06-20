using CVTech.BuildingBlocks.Exceptions;
using CVTech.CatalogueEmploi.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CVTech.CatalogueEmploi.Application.Features.ConsulterAnnonce;

public sealed class ConsulterAnnonceHandler : IRequestHandler<ConsulterAnnonceQuery, AnnonceDetail>
{
    private readonly CatalogueEmploiDbContext _dbContext;

    public ConsulterAnnonceHandler(CatalogueEmploiDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<AnnonceDetail> Handle(ConsulterAnnonceQuery request, CancellationToken cancellationToken)
    {
        var annonce = await _dbContext.Annonces
            .Where(a => a.Id == request.AnnonceId && !a.EstSupprimee)
            .Select(a => new AnnonceDetail(a.Id, a.Titre, a.Description, a.TypeContrat, a.DomaineMetierCode, a.Localisation, a.EntrepriseId, a.DateCreation))
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new EntiteIntrouvableException("AnnonceEmploi", request.AnnonceId);

        return annonce;
    }
}
