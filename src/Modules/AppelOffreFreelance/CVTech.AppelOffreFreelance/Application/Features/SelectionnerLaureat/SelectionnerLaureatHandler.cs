using CVTech.AppelOffreFreelance.Domaine.Exceptions;
using CVTech.AppelOffreFreelance.Infrastructure;
using CVTech.BuildingBlocks.Exceptions;
using CVTech.BuildingBlocks.Permissions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CVTech.AppelOffreFreelance.Application.Features.SelectionnerLaureat;

public sealed class SelectionnerLaureatHandler : IRequestHandler<SelectionnerLaureatCommand>
{
    private readonly AppelOffreFreelanceDbContext _dbContext;
    private readonly IVerificateurPermission _verificateurPermission;

    public SelectionnerLaureatHandler(AppelOffreFreelanceDbContext dbContext,
        IVerificateurPermission verificateurPermission)
    {
        _dbContext = dbContext;
        _verificateurPermission = verificateurPermission;
    }

    public async Task Handle(SelectionnerLaureatCommand request, CancellationToken cancellationToken)
    {
        _verificateurPermission.VerifierPermission(request.RoleActeur, Permissions.SelectionnerLaureat);

        var appelOffre = await _dbContext.AppelsOffres
            .FirstOrDefaultAsync(a => a.Id == request.AppelOffreId && !a.EstSupprime, cancellationToken)
            ?? throw new EntiteIntrouvableException("AppelOffre", request.AppelOffreId);

        if (request.RoleActeur != RoleUtilisateur.Administrateur && appelOffre.EntrepriseId != request.EntrepriseId)
            throw new PermissionRefuseeException(Permissions.SelectionnerLaureat);

        if (appelOffre.LaureatId.HasValue)
            throw new LaureatDejaSelectionneException();

        var proposition = await _dbContext.Propositions
            .FirstOrDefaultAsync(p => p.Id == request.PropositionId && p.AppelOffreId == request.AppelOffreId, cancellationToken)
            ?? throw new EntiteIntrouvableException("PropositionFreelance", request.PropositionId);

        appelOffre.SelectionnerLaureat(proposition.Id);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
