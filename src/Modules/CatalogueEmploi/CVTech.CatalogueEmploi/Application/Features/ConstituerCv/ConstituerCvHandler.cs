using CVTech.BuildingBlocks.Permissions;
using CVTech.CatalogueEmploi.Domaine;
using CVTech.CatalogueEmploi.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CVTech.CatalogueEmploi.Application.Features.ConstituerCv;

public sealed class ConstituerCvHandler : IRequestHandler<ConstituerCvCommand, ConstituerCvResultat>
{
    private readonly CatalogueEmploiDbContext _dbContext;
    private readonly IVerificateurPermission _verificateurPermission;

    public ConstituerCvHandler(CatalogueEmploiDbContext dbContext, IVerificateurPermission verificateurPermission)
    {
        _dbContext = dbContext;
        _verificateurPermission = verificateurPermission;
    }

    public async Task<ConstituerCvResultat> Handle(ConstituerCvCommand request, CancellationToken cancellationToken)
    {
        _verificateurPermission.VerifierPermission(request.RoleActeur, Permissions.ConstituerCv);

        var cv = await _dbContext.CurriculumsVitae
            .FirstOrDefaultAsync(c => c.CandidatId == request.CandidatId, cancellationToken);

        if (cv is null)
        {
            cv = CurriculumVitae.Constituer(request.CandidatId, request.Titre, request.Resume, request.Competences);
            _dbContext.CurriculumsVitae.Add(cv);
        }
        else
        {
            cv.Modifier(request.Titre, request.Resume, request.Competences);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new ConstituerCvResultat(cv.Id);
    }
}
