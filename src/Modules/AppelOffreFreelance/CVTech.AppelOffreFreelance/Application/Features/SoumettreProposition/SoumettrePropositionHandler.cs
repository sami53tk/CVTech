using CVTech.AppelOffreFreelance.Domaine;
using CVTech.AppelOffreFreelance.Domaine.Exceptions;
using CVTech.AppelOffreFreelance.Infrastructure;
using CVTech.BuildingBlocks.Exceptions;
using CVTech.BuildingBlocks.Permissions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CVTech.AppelOffreFreelance.Application.Features.SoumettreProposition;

public sealed class SoumettrePropositionHandler : IRequestHandler<SoumettrePropositionCommand, SoumettrePropositionResultat>
{
    private readonly AppelOffreFreelanceDbContext _dbContext;
    private readonly IVerificateurPermission _verificateurPermission;

    public SoumettrePropositionHandler(AppelOffreFreelanceDbContext dbContext,
        IVerificateurPermission verificateurPermission)
    {
        _dbContext = dbContext;
        _verificateurPermission = verificateurPermission;
    }

    public async Task<SoumettrePropositionResultat> Handle(SoumettrePropositionCommand request, CancellationToken cancellationToken)
    {
        _verificateurPermission.VerifierPermission(request.RoleActeur, Permissions.SoumettrePropositionFreelance);

        var appelOffre = await _dbContext.AppelsOffres
            .FirstOrDefaultAsync(a => a.Id == request.AppelOffreId && !a.EstSupprime, cancellationToken)
            ?? throw new EntiteIntrouvableException("AppelOffre", request.AppelOffreId);

        var dejasoumis = await _dbContext.Propositions
            .AnyAsync(p => p.AppelOffreId == request.AppelOffreId && p.FreelanceId == request.FreelanceId, cancellationToken);

        if (dejasoumis)
            throw new PropositionDejaSubmiseException();

        var proposition = PropositionFreelance.Soumettre(
            request.AppelOffreId, request.FreelanceId,
            request.Description, request.TauxJournalier, request.DureeEstimeeJours);

        _dbContext.Propositions.Add(proposition);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new SoumettrePropositionResultat(proposition.Id);
    }
}
