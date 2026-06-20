using CVTech.BuildingBlocks.Permissions;
using MediatR;

namespace CVTech.CatalogueEmploi.Application.Features.ModererAnnonce;

public sealed record ModererAnnonceCommand(RoleUtilisateur RoleActeur, Guid AnnonceId) : IRequest;
