using CVTech.BuildingBlocks.Permissions;
using MediatR;

namespace CVTech.CatalogueEmploi.Application.Features.ConsulterCandidaturesRecues;

public sealed record ConsulterCandidaturesRecuesQuery(RoleUtilisateur RoleActeur, Guid EntrepriseId, Guid AnnonceId) : IRequest<IReadOnlyCollection<CandidatureResume>>;

public sealed record CandidatureResume(Guid Id, Guid CandidatId, string? Message, DateTimeOffset DateCandidature);
