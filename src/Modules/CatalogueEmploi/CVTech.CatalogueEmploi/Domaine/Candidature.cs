using CVTech.BuildingBlocks.Domaine;

namespace CVTech.CatalogueEmploi.Domaine;

/// <summary>
/// Candidature déposée par un candidat sur une annonce d'emploi. Agrégat racine du module
/// CatalogueEmploi, référence l'annonce et le candidat par identifiant uniquement.
/// </summary>
public sealed class Candidature : AgregatRacine
{
    public Guid AnnonceEmploiId { get; private set; }
    public Guid CandidatId { get; private set; }
    public string? Message { get; private set; }

    private Candidature()
    {
    }

    public static Candidature Soumettre(Guid annonceEmploiId, Guid candidatId, string? message)
    {
        return new Candidature
        {
            AnnonceEmploiId = annonceEmploiId,
            CandidatId = candidatId,
            Message = message,
        };
    }
}
