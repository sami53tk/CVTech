using MediatR;

namespace CVTech.AppelOffreFreelance.Application.Features.ConsulterAppelOffre;

public sealed record ConsulterAppelOffreQuery(Guid AppelOffreId) : IRequest<AppelOffreDetail>;

public sealed record AppelOffreDetail(
    Guid Id,
    string Titre,
    string Description,
    string DomaineMetierCode,
    string Localisation,
    decimal BudgetMax,
    Guid EntrepriseId,
    DateTimeOffset DatePublication,
    Guid? LaureatId);
