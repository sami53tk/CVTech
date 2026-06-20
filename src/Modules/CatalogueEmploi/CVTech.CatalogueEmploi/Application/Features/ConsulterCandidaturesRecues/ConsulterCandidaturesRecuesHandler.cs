using CVTech.BuildingBlocks.Exceptions;
using CVTech.BuildingBlocks.Permissions;
using CVTech.CatalogueEmploi.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CVTech.CatalogueEmploi.Application.Features.ConsulterCandidaturesRecues;

public sealed class ConsulterCandidaturesRecuesHandler : IRequestHandler<ConsulterCandidaturesRecuesQuery, IReadOnlyCollection<CandidatureResume>>
{
    private readonly CatalogueEmploiDbContext _dbContext;
    private readonly IVerificateurPermission _verificateurPermission;

    public ConsulterCandidaturesRecuesHandler(CatalogueEmploiDbContext dbContext, IVerificateurPermission verificateurPermission)
    {
        _dbContext = dbContext;
        _verificateurPermission = verificateurPermission;
    }

    public async Task<IReadOnlyCollection<CandidatureResume>> Handle(ConsulterCandidaturesRecuesQuery request, CancellationToken cancellationToken)
    {
        _verificateurPermission.VerifierPermission(request.RoleActeur, Permissions.ConsulterCandidaturesRecues);

        var annonce = await _dbContext.Annonces.FindAsync([request.AnnonceId], cancellationToken)
            ?? throw new EntiteIntrouvableException("AnnonceEmploi", request.AnnonceId);

        if (request.RoleActeur != RoleUtilisateur.Administrateur && annonce.EntrepriseId != request.EntrepriseId)
        {
            throw new PermissionRefuseeException(Permissions.ConsulterCandidaturesRecues);
        }

        var candidatures = await _dbContext.Candidatures
            .Where(c => c.AnnonceEmploiId == request.AnnonceId)
            .Select(c => new CandidatureResume(c.Id, c.CandidatId, c.Message, c.DateCreation))
            .ToListAsync(cancellationToken);

        // Tri en mémoire : SQLite ne sait pas trier par DateTimeOffset côté serveur.
        return candidatures.OrderByDescending(c => c.DateCandidature).ToList();
    }
}
