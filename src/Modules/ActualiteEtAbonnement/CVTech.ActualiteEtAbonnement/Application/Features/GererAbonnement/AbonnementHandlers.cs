using CVTech.ActualiteEtAbonnement.Domaine;
using CVTech.ActualiteEtAbonnement.Infrastructure;
using CVTech.BuildingBlocks.Exceptions;
using CVTech.BuildingBlocks.Permissions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CVTech.ActualiteEtAbonnement.Application.Features.GererAbonnement;

public sealed class SAbonnerHandler : IRequestHandler<SAbonnerCommand, Guid>
{
    private readonly ActualiteEtAbonnementDbContext _dbContext;
    private readonly IVerificateurPermission _verificateurPermission;

    public SAbonnerHandler(ActualiteEtAbonnementDbContext dbContext, IVerificateurPermission verificateurPermission)
    {
        _dbContext = dbContext;
        _verificateurPermission = verificateurPermission;
    }

    public async Task<Guid> Handle(SAbonnerCommand request, CancellationToken cancellationToken)
    {
        _verificateurPermission.VerifierPermission(request.RoleActeur, Permissions.SAbonnerDomaineMetier);

        var dejaAbonne = await _dbContext.Abonnements.AnyAsync(
            a => a.UtilisateurId == request.UtilisateurId
              && a.DomaineMetierCode == request.DomaineMetierCode.Trim().ToLowerInvariant(),
            cancellationToken);

        if (dejaAbonne)
        {
            var existant = await _dbContext.Abonnements
                .FirstAsync(a => a.UtilisateurId == request.UtilisateurId
                              && a.DomaineMetierCode == request.DomaineMetierCode.Trim().ToLowerInvariant(),
                    cancellationToken);
            return existant.Id;
        }

        var abonnement = Abonnement.Creer(request.UtilisateurId, request.DomaineMetierCode);
        _dbContext.Abonnements.Add(abonnement);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return abonnement.Id;
    }
}

public sealed class SeDesabonnerHandler : IRequestHandler<SeDesabonnerCommand>
{
    private readonly ActualiteEtAbonnementDbContext _dbContext;
    private readonly IVerificateurPermission _verificateurPermission;

    public SeDesabonnerHandler(ActualiteEtAbonnementDbContext dbContext, IVerificateurPermission verificateurPermission)
    {
        _dbContext = dbContext;
        _verificateurPermission = verificateurPermission;
    }

    public async Task Handle(SeDesabonnerCommand request, CancellationToken cancellationToken)
    {
        _verificateurPermission.VerifierPermission(request.RoleActeur, Permissions.SAbonnerDomaineMetier);

        var abonnement = await _dbContext.Abonnements
            .FirstOrDefaultAsync(a => a.Id == request.AbonnementId && a.UtilisateurId == request.UtilisateurId, cancellationToken)
            ?? throw new EntiteIntrouvableException("Abonnement", request.AbonnementId);

        _dbContext.Abonnements.Remove(abonnement);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}

public sealed class ConsulterAbonnementsHandler : IRequestHandler<ConsulterAbonnementsQuery, IReadOnlyCollection<AbonnementResume>>
{
    private readonly ActualiteEtAbonnementDbContext _dbContext;

    public ConsulterAbonnementsHandler(ActualiteEtAbonnementDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<AbonnementResume>> Handle(ConsulterAbonnementsQuery request, CancellationToken cancellationToken)
    {
        return await _dbContext.Abonnements
            .Where(a => a.UtilisateurId == request.UtilisateurId)
            .Select(a => new AbonnementResume(a.Id, a.DomaineMetierCode, a.DateCreation))
            .ToListAsync(cancellationToken);
    }
}
