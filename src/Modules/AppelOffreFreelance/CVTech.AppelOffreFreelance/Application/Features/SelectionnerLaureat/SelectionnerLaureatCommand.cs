using CVTech.BuildingBlocks.Permissions;
using MediatR;

namespace CVTech.AppelOffreFreelance.Application.Features.SelectionnerLaureat;

public sealed record SelectionnerLaureatCommand(
    RoleUtilisateur RoleActeur,
    Guid EntrepriseId,
    Guid AppelOffreId,
    Guid PropositionId
) : IRequest;
