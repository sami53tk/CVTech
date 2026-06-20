using CVTech.BuildingBlocks.Permissions;
using MediatR;

namespace CVTech.CatalogueEmploi.Application.Features.PostulerAnnonce;

public sealed record PostulerAnnonceCommand(RoleUtilisateur RoleActeur, Guid CandidatId, Guid AnnonceId, string? Message) : IRequest<PostulerAnnonceResultat>;

public sealed record PostulerAnnonceResultat(Guid CandidatureId);
