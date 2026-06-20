using CVTech.BuildingBlocks.Domaine;

namespace CVTech.CatalogueEmploi.Domaine;

/// <summary>
/// CV constitué par un candidat. Agrégat racine du module CatalogueEmploi, un par candidat.
/// </summary>
public sealed class CurriculumVitae : AgregatRacine
{
    public Guid CandidatId { get; private set; }
    public string Titre { get; private set; } = null!;
    public string Resume { get; private set; } = null!;
    public List<string> Competences { get; private set; } = [];
    public DateTimeOffset DateDerniereModification { get; private set; }

    private CurriculumVitae()
    {
    }

    public static CurriculumVitae Constituer(Guid candidatId, string titre, string resume, IEnumerable<string> competences)
    {
        return new CurriculumVitae
        {
            CandidatId = candidatId,
            Titre = titre,
            Resume = resume,
            Competences = competences.ToList(),
            DateDerniereModification = DateTimeOffset.UtcNow,
        };
    }

    public void Modifier(string titre, string resume, IEnumerable<string> competences)
    {
        Titre = titre;
        Resume = resume;
        Competences = competences.ToList();
        DateDerniereModification = DateTimeOffset.UtcNow;
    }
}
