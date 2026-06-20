using CVTech.BuildingBlocks.Domaine;
using CVTech.BuildingBlocks.Permissions;
using MediatR;

namespace CVTech.ActualiteEtAbonnement.Application.Features.GererDomaines;

public sealed record CreerDomaineCommand(RoleUtilisateur RoleActeur, string Code, string Libelle) : IRequest<Guid>;

public sealed record SupprimerDomaineCommand(RoleUtilisateur RoleActeur, Guid DomaineId) : IRequest;

public sealed record ListerDomainesQuery : IRequest<IReadOnlyCollection<DomaineMetier>>;
