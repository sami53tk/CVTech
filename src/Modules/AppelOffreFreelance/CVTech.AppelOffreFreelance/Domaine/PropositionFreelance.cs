using CVTech.BuildingBlocks.Domaine;

namespace CVTech.AppelOffreFreelance.Domaine;

public sealed class PropositionFreelance : AgregatRacine
{
    public Guid AppelOffreId { get; private set; }
    public Guid FreelanceId { get; private set; }
    public string Description { get; private set; } = null!;
    public decimal TauxJournalier { get; private set; }
    public int DureeEstimeeJours { get; private set; }

    private PropositionFreelance() { }

    public static PropositionFreelance Soumettre(Guid appelOffreId, Guid freelanceId,
        string description, decimal tauxJournalier, int dureeEstimeeJours)
    {
        return new PropositionFreelance
        {
            Id = Guid.NewGuid(),
            AppelOffreId = appelOffreId,
            FreelanceId = freelanceId,
            Description = description,
            TauxJournalier = tauxJournalier,
            DureeEstimeeJours = dureeEstimeeJours,
            DateCreation = DateTimeOffset.UtcNow,
        };
    }
}
