using CVTech.BuildingBlocks.Exceptions;
using CVTech.BuildingBlocks.Permissions;
using CVTech.CatalogueEmploi.Domaine;
using CVTech.CatalogueEmploi.Domaine.Exceptions;
using CVTech.CatalogueEmploi.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CVTech.CatalogueEmploi.Application.Features.PostulerAnnonce;

public sealed class PostulerAnnonceHandler : IRequestHandler<PostulerAnnonceCommand, PostulerAnnonceResultat>
{
    private readonly CatalogueEmploiDbContext _dbContext;
    private readonly IVerificateurPermission _verificateurPermission;

    public PostulerAnnonceHandler(CatalogueEmploiDbContext dbContext, IVerificateurPermission verificateurPermission)
    {
        _dbContext = dbContext;
        _verificateurPermission = verificateurPermission;
    }

    public async Task<PostulerAnnonceResultat> Handle(PostulerAnnonceCommand request, CancellationToken cancellationToken)
    {
        _verificateurPermission.VerifierPermission(request.RoleActeur, Permissions.PostulerAnnonce);

        var annonceExiste = await _dbContext.Annonces
            .AnyAsync(a => a.Id == request.AnnonceId && !a.EstSupprimee, cancellationToken);
        if (!annonceExiste)
        {
            throw new EntiteIntrouvableException("AnnonceEmploi", request.AnnonceId);
        }

        var possedeUnCv = await _dbContext.CurriculumsVitae
            .AnyAsync(c => c.CandidatId == request.CandidatId, cancellationToken);
        if (!possedeUnCv)
        {
            throw new CvObligatoirePourPostulerException();
        }

        var aDejaPostule = await _dbContext.Candidatures
            .AnyAsync(c => c.AnnonceEmploiId == request.AnnonceId && c.CandidatId == request.CandidatId, cancellationToken);
        if (aDejaPostule)
        {
            throw new CandidatureDejaSoumiseException();
        }

        var candidature = Candidature.Soumettre(request.AnnonceId, request.CandidatId, request.Message);
        _dbContext.Candidatures.Add(candidature);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new PostulerAnnonceResultat(candidature.Id);
    }
}
