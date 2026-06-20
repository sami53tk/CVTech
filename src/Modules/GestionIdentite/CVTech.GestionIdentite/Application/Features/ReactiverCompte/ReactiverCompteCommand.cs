using CVTech.BuildingBlocks.Permissions;
using MediatR;

namespace CVTech.GestionIdentite.Application.Features.ReactiverCompte;

public sealed record ReactiverCompteCommand(RoleUtilisateur RoleActeur, Guid CompteId) : IRequest;
