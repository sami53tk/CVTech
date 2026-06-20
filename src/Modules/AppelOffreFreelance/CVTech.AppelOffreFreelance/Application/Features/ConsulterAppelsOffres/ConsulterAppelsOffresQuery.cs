using MediatR;

namespace CVTech.AppelOffreFreelance.Application.Features.ConsulterAppelsOffres;

public sealed record ConsulterAppelsOffresQuery(string? DomaineMetierCode) : IRequest<IReadOnlyCollection<AppelOffreResume>>;

public sealed record AppelOffreResume(
    Guid Id,
    string Titre,
    string DomaineMetierCode,
    string Localisation,
    decimal BudgetMax,
    Guid EntrepriseId,
    DateTimeOffset DatePublication,
    bool LaureatSelectionne);
