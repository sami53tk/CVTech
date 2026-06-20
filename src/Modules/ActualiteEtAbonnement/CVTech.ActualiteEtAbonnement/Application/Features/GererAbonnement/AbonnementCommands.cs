using CVTech.BuildingBlocks.Permissions;
using MediatR;

namespace CVTech.ActualiteEtAbonnement.Application.Features.GererAbonnement;

public sealed record SAbonnerCommand(RoleUtilisateur RoleActeur, Guid UtilisateurId, string DomaineMetierCode) : IRequest<Guid>;

public sealed record SeDesabonnerCommand(RoleUtilisateur RoleActeur, Guid UtilisateurId, Guid AbonnementId) : IRequest;

public sealed record ConsulterAbonnementsQuery(Guid UtilisateurId) : IRequest<IReadOnlyCollection<AbonnementResume>>;

public sealed record AbonnementResume(Guid Id, string DomaineMetierCode, DateTimeOffset DateAbonnement);
