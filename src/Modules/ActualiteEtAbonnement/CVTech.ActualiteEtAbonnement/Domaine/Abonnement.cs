using CVTech.BuildingBlocks.Domaine;

namespace CVTech.ActualiteEtAbonnement.Domaine;

public sealed class Abonnement : AgregatRacine
{
    public Guid UtilisateurId { get; private set; }
    public string DomaineMetierCode { get; private set; } = null!;

    private Abonnement() { }

    public static Abonnement Creer(Guid utilisateurId, string domaineMetierCode)
    {
        return new Abonnement
        {
            Id = Guid.NewGuid(),
            UtilisateurId = utilisateurId,
            DomaineMetierCode = domaineMetierCode.Trim().ToLowerInvariant(),
            DateCreation = DateTimeOffset.UtcNow,
        };
    }
}
