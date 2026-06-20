using CVTech.BuildingBlocks.Domaine;

namespace CVTech.ActualiteEtAbonnement.Domaine;

public sealed class Notification : AgregatRacine
{
    public Guid UtilisateurId { get; private set; }
    public string Message { get; private set; } = null!;
    public string DomaineMetierCode { get; private set; } = null!;
    public string TypeSource { get; private set; } = null!;
    public Guid SourceId { get; private set; }
    public bool EstLue { get; private set; }

    private Notification() { }

    public static Notification Creer(Guid utilisateurId, string message,
        string domaineMetierCode, string typeSource, Guid sourceId)
    {
        return new Notification
        {
            Id = Guid.NewGuid(),
            UtilisateurId = utilisateurId,
            Message = message,
            DomaineMetierCode = domaineMetierCode,
            TypeSource = typeSource,
            SourceId = sourceId,
            EstLue = false,
            DateCreation = DateTimeOffset.UtcNow,
        };
    }
}
