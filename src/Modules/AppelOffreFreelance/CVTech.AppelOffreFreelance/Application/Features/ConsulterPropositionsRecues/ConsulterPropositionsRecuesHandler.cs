using CVTech.AppelOffreFreelance.Infrastructure;
using CVTech.BuildingBlocks.Exceptions;
using CVTech.BuildingBlocks.Permissions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CVTech.AppelOffreFreelance.Application.Features.ConsulterPropositionsRecues;

public sealed class ConsulterPropositionsRecuesHandler
    : IRequestHandler<ConsulterPropositionsRecuesQuery, IReadOnlyCollection<PropositionResume>>
{
    private readonly AppelOffreFreelanceDbContext _dbContext;
    private readonly IVerificateurPermission _verificateurPermission;

    public ConsulterPropositionsRecuesHandler(AppelOffreFreelanceDbContext dbContext,
        IVerificateurPermission verificateurPermission)
    {
        _dbContext = dbContext;
        _verificateurPermission = verificateurPermission;
    }

    public async Task<IReadOnlyCollection<PropositionResume>> Handle(
        ConsulterPropositionsRecuesQuery request, CancellationToken cancellationToken)
    {
        _verificateurPermission.VerifierPermission(request.RoleActeur, Permissions.ConsulterPropositionsRecues);

        var appelOffre = await _dbContext.AppelsOffres
            .FirstOrDefaultAsync(a => a.Id == request.AppelOffreId, cancellationToken)
            ?? throw new EntiteIntrouvableException("AppelOffre", request.AppelOffreId);

        if (request.RoleActeur != RoleUtilisateur.Administrateur && appelOffre.EntrepriseId != request.EntrepriseId)
            throw new PermissionRefuseeException(Permissions.ConsulterPropositionsRecues);

        var propositions = await _dbContext.Propositions
            .Where(p => p.AppelOffreId == request.AppelOffreId)
            .Select(p => new PropositionResume(p.Id, p.FreelanceId, p.Description,
                p.TauxJournalier, p.DureeEstimeeJours, p.DateCreation))
            .ToListAsync(cancellationToken);

        // Tri en mémoire : SQLite ne sait pas trier par DateTimeOffset côté serveur.
        return propositions.OrderByDescending(p => p.DateSoumission).ToList();
    }
}
