using CVTech.BuildingBlocks.Permissions;
using MediatR;

namespace CVTech.AppelOffreFreelance.Application.Features.PublierAppelOffre;

public sealed record PublierAppelOffreCommand(
    RoleUtilisateur RoleActeur,
    Guid EntrepriseId,
    string Titre,
    string Description,
    string DomaineMetierCode,
    string Localisation,
    decimal BudgetMax
) : IRequest<PublierAppelOffreResultat>;

public sealed record PublierAppelOffreResultat(Guid AppelOffreId);
