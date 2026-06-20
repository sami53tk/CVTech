using CVTech.CatalogueEmploi.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CVTech.CatalogueEmploi.Application.Features.ConsulterMonCv;

public sealed class ConsulterMonCvHandler : IRequestHandler<ConsulterMonCvQuery, CvResultat?>
{
    private readonly CatalogueEmploiDbContext _dbContext;

    public ConsulterMonCvHandler(CatalogueEmploiDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CvResultat?> Handle(ConsulterMonCvQuery request, CancellationToken cancellationToken)
    {
        return await _dbContext.CurriculumsVitae
            .Where(c => c.CandidatId == request.CandidatId)
            .Select(c => new CvResultat(c.Id, c.Titre, c.Resume, c.Competences, c.DateDerniereModification))
            .FirstOrDefaultAsync(cancellationToken);
    }
}
