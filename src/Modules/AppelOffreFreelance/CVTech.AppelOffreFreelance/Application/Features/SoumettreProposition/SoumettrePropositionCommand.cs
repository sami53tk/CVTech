using CVTech.BuildingBlocks.Permissions;
using MediatR;

namespace CVTech.AppelOffreFreelance.Application.Features.SoumettreProposition;

public sealed record SoumettrePropositionCommand(
    RoleUtilisateur RoleActeur,
    Guid FreelanceId,
    Guid AppelOffreId,
    string Description,
    decimal TauxJournalier,
    int DureeEstimeeJours
) : IRequest<SoumettrePropositionResultat>;

public sealed record SoumettrePropositionResultat(Guid PropositionId);
