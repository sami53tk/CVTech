using CVTech.BuildingBlocks.Permissions;
using MediatR;

namespace CVTech.GestionIdentite.Application.Features.BloquerCompte;

public sealed record BloquerCompteCommand(RoleUtilisateur RoleActeur, Guid CompteId) : IRequest;
