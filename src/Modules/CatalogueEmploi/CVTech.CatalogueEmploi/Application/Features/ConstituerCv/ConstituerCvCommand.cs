using CVTech.BuildingBlocks.Permissions;
using MediatR;

namespace CVTech.CatalogueEmploi.Application.Features.ConstituerCv;

public sealed record ConstituerCvCommand(
    RoleUtilisateur RoleActeur,
    Guid CandidatId,
    string Titre,
    string Resume,
    IReadOnlyCollection<string> Competences
) : IRequest<ConstituerCvResultat>;

public sealed record ConstituerCvResultat(Guid CvId);
