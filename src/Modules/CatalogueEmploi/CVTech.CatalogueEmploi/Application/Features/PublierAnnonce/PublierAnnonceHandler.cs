using CVTech.BuildingBlocks.Evenements;
using CVTech.BuildingBlocks.Permissions;
using CVTech.CatalogueEmploi.Domaine;
using CVTech.CatalogueEmploi.Infrastructure;
using MediatR;

namespace CVTech.CatalogueEmploi.Application.Features.PublierAnnonce;

public sealed class PublierAnnonceHandler : IRequestHandler<PublierAnnonceCommand, PublierAnnonceResultat>
{
    private readonly CatalogueEmploiDbContext _dbContext;
    private readonly IVerificateurPermission _verificateurPermission;
    private readonly IBusEvenements _busEvenements;

    public PublierAnnonceHandler(
        CatalogueEmploiDbContext dbContext,
        IVerificateurPermission verificateurPermission,
        IBusEvenements busEvenements)
    {
        _dbContext = dbContext;
        _verificateurPermission = verificateurPermission;
        _busEvenements = busEvenements;
    }

    public async Task<PublierAnnonceResultat> Handle(PublierAnnonceCommand request, CancellationToken cancellationToken)
    {
        _verificateurPermission.VerifierPermission(request.RoleActeur, Permissions.PublierAnnonce);

        var annonce = AnnonceEmploi.Publier(
            request.Titre,
            request.Description,
            request.TypeContrat,
            request.DomaineMetierCode,
            request.Localisation,
            request.EntrepriseId);

        _dbContext.Annonces.Add(annonce);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _busEvenements.PublierAsync(
            new AnnoncePublieeEvent(annonce.Id, annonce.Titre, annonce.DomaineMetierCode, annonce.EntrepriseId, annonce.DateCreation),
            cancellationToken);

        return new PublierAnnonceResultat(annonce.Id);
    }
}
