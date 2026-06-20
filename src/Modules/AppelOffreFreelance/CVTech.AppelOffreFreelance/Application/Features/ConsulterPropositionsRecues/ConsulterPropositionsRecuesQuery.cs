using CVTech.BuildingBlocks.Permissions;
using MediatR;

namespace CVTech.AppelOffreFreelance.Application.Features.ConsulterPropositionsRecues;

public sealed record ConsulterPropositionsRecuesQuery(
    RoleUtilisateur RoleActeur,
    Guid EntrepriseId,
    Guid AppelOffreId
) : IRequest<IReadOnlyCollection<PropositionResume>>;

public sealed record PropositionResume(
    Guid Id,
    Guid FreelanceId,
    string Description,
    decimal TauxJournalier,
    int DureeEstimeeJours,
    DateTimeOffset DateSoumission);
