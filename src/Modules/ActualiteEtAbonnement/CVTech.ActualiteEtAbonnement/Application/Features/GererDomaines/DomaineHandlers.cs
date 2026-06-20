using CVTech.ActualiteEtAbonnement.Domaine;
using CVTech.ActualiteEtAbonnement.Infrastructure;
using CVTech.BuildingBlocks.Domaine;
using CVTech.BuildingBlocks.Exceptions;
using CVTech.BuildingBlocks.Permissions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CVTech.ActualiteEtAbonnement.Application.Features.GererDomaines;

public sealed class CreerDomaineHandler : IRequestHandler<CreerDomaineCommand, Guid>
{
    private readonly ActualiteEtAbonnementDbContext _dbContext;
    private readonly IVerificateurPermission _verificateurPermission;

    public CreerDomaineHandler(ActualiteEtAbonnementDbContext dbContext, IVerificateurPermission verificateurPermission)
    {
        _dbContext = dbContext;
        _verificateurPermission = verificateurPermission;
    }

    public async Task<Guid> Handle(CreerDomaineCommand request, CancellationToken cancellationToken)
    {
        _verificateurPermission.VerifierPermission(request.RoleActeur, Permissions.GererReferentielDomaines);

        var domaine = DomaineMetierReference.Creer(request.Code, request.Libelle);
        _dbContext.DomainesMetier.Add(domaine);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return domaine.Id;
    }
}

public sealed class SupprimerDomaineHandler : IRequestHandler<SupprimerDomaineCommand>
{
    private readonly ActualiteEtAbonnementDbContext _dbContext;
    private readonly IVerificateurPermission _verificateurPermission;

    public SupprimerDomaineHandler(ActualiteEtAbonnementDbContext dbContext, IVerificateurPermission verificateurPermission)
    {
        _dbContext = dbContext;
        _verificateurPermission = verificateurPermission;
    }

    public async Task Handle(SupprimerDomaineCommand request, CancellationToken cancellationToken)
    {
        _verificateurPermission.VerifierPermission(request.RoleActeur, Permissions.GererReferentielDomaines);

        var domaine = await _dbContext.DomainesMetier.FindAsync([request.DomaineId], cancellationToken)
            ?? throw new EntiteIntrouvableException("DomaineMetier", request.DomaineId);

        _dbContext.DomainesMetier.Remove(domaine);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}

public sealed class ListerDomainesHandler : IRequestHandler<ListerDomainesQuery, IReadOnlyCollection<DomaineMetier>>
{
    private readonly IReferentielDomaineMetier _referentiel;

    public ListerDomainesHandler(IReferentielDomaineMetier referentiel)
    {
        _referentiel = referentiel;
    }

    public Task<IReadOnlyCollection<DomaineMetier>> Handle(ListerDomainesQuery request, CancellationToken cancellationToken)
        => _referentiel.ListerAsync(cancellationToken);
}
