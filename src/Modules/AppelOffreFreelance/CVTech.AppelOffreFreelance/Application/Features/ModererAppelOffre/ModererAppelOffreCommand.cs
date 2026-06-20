using CVTech.BuildingBlocks.Permissions;
using MediatR;

namespace CVTech.AppelOffreFreelance.Application.Features.ModererAppelOffre;

public sealed record ModererAppelOffreCommand(RoleUtilisateur RoleActeur, Guid AppelOffreId) : IRequest;
