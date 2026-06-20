using CVTech.AppelOffreFreelance.Infrastructure;
using CVTech.BuildingBlocks.Exceptions;
using CVTech.BuildingBlocks.Permissions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CVTech.AppelOffreFreelance.Application.Features.ModererAppelOffre;

public sealed class ModererAppelOffreHandler : IRequestHandler<ModererAppelOffreCommand>
{
    private readonly AppelOffreFreelanceDbContext _dbContext;
    private readonly IVerificateurPermission _verificateurPermission;

    public ModererAppelOffreHandler(AppelOffreFreelanceDbContext dbContext,
        IVerificateurPermission verificateurPermission)
    {
        _dbContext = dbContext;
        _verificateurPermission = verificateurPermission;
    }

    public async Task Handle(ModererAppelOffreCommand request, CancellationToken cancellationToken)
    {
        _verificateurPermission.VerifierPermission(request.RoleActeur, Permissions.ModererAnnonceOuAppelOffre);

        var appelOffre = await _dbContext.AppelsOffres
            .FirstOrDefaultAsync(a => a.Id == request.AppelOffreId, cancellationToken)
            ?? throw new EntiteIntrouvableException("AppelOffre", request.AppelOffreId);

        appelOffre.Moderer();
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
