namespace CVTech.AppelOffreFreelance.Client.Dto;

public sealed record PublierAppelOffreRequete(
    string Titre,
    string Description,
    string DomaineMetierCode,
    string Localisation,
    decimal BudgetMax);

public sealed record SoumettrePropositionRequete(
    string Description,
    decimal TauxJournalier,
    int DureeEstimeeJours);

public sealed record SelectionnerLaureatRequete(Guid PropositionId);
