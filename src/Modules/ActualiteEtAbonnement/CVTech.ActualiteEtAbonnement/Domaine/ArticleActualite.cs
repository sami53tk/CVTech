using CVTech.BuildingBlocks.Domaine;

namespace CVTech.ActualiteEtAbonnement.Domaine;

public sealed class ArticleActualite : AgregatRacine
{
    public string Titre { get; private set; } = null!;
    public string Contenu { get; private set; } = null!;
    public string? DomaineMetierCode { get; private set; }
    public Guid AuteurId { get; private set; }
    public string LienExterne { get; private set; } = null!;

    private ArticleActualite() { }

    public static ArticleActualite Publier(string titre, string contenu,
        string? domaineMetierCode, Guid auteurId, string lienExterne)
    {
        return new ArticleActualite
        {
            Id = Guid.NewGuid(),
            Titre = titre,
            Contenu = contenu,
            DomaineMetierCode = domaineMetierCode,
            AuteurId = auteurId,
            LienExterne = lienExterne,
            DateCreation = DateTimeOffset.UtcNow,
        };
    }
}
