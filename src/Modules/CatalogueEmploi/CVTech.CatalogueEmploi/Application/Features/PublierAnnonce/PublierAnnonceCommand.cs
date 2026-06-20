using CVTech.BuildingBlocks.Permissions;
using CVTech.CatalogueEmploi.Domaine;
using MediatR;

namespace CVTech.CatalogueEmploi.Application.Features.PublierAnnonce;

public sealed record PublierAnnonceCommand(
    RoleUtilisateur RoleActeur,
    Guid EntrepriseId,
    string Titre,
    string Description,
    TypeContrat TypeContrat,
    string DomaineMetierCode,
    string Localisation
) : IRequest<PublierAnnonceResultat>;

public sealed record PublierAnnonceResultat(Guid AnnonceId);
