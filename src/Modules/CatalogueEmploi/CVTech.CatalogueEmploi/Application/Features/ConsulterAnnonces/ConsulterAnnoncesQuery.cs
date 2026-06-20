using CVTech.CatalogueEmploi.Domaine;
using MediatR;

namespace CVTech.CatalogueEmploi.Application.Features.ConsulterAnnonces;

public sealed record ConsulterAnnoncesQuery(string? DomaineMetierCode, TypeContrat? TypeContrat) : IRequest<IReadOnlyCollection<AnnonceResume>>;

public sealed record AnnonceResume(
    Guid Id,
    string Titre,
    TypeContrat TypeContrat,
    string DomaineMetierCode,
    string Localisation,
    Guid EntrepriseId,
    DateTimeOffset DatePublication
);
