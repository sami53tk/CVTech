using CVTech.CatalogueEmploi.Domaine;
using MediatR;

namespace CVTech.CatalogueEmploi.Application.Features.ConsulterAnnonce;

public sealed record ConsulterAnnonceQuery(Guid AnnonceId) : IRequest<AnnonceDetail>;

public sealed record AnnonceDetail(
    Guid Id,
    string Titre,
    string Description,
    TypeContrat TypeContrat,
    string DomaineMetierCode,
    string Localisation,
    Guid EntrepriseId,
    DateTimeOffset DatePublication
);
