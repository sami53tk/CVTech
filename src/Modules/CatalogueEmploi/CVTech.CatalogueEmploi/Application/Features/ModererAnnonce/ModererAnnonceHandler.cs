using CVTech.BuildingBlocks.Exceptions;
using CVTech.BuildingBlocks.Permissions;
using CVTech.CatalogueEmploi.Infrastructure;
using MediatR;

namespace CVTech.CatalogueEmploi.Application.Features.ModererAnnonce;

public sealed class ModererAnnonceHandler : IRequestHandler<ModererAnnonceCommand>
{
    private readonly CatalogueEmploiDbContext _dbContext;
    private readonly IVerificateurPermission _verificateurPermission;

    public ModererAnnonceHandler(CatalogueEmploiDbContext dbContext, IVerificateurPermission verificateurPermission)
    {
        _dbContext = dbContext;
        _verificateurPermission = verificateurPermission;
    }

    public async Task Handle(ModererAnnonceCommand request, CancellationToken cancellationToken)
    {
        _verificateurPermission.VerifierPermission(request.RoleActeur, Permissions.ModererAnnonceOuAppelOffre);

        var annonce = await _dbContext.Annonces.FindAsync([request.AnnonceId], cancellationToken)
            ?? throw new EntiteIntrouvableException("AnnonceEmploi", request.AnnonceId);

        annonce.Moderer();
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
