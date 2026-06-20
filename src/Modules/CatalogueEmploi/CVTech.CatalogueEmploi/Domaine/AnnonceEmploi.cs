using CVTech.BuildingBlocks.Domaine;

namespace CVTech.CatalogueEmploi.Domaine;

/// <summary>
/// Annonce d'emploi publiée par une entreprise. Agrégat racine du module CatalogueEmploi.
/// </summary>
public sealed class AnnonceEmploi : AgregatRacine
{
    public string Titre { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public TypeContrat TypeContrat { get; private set; }
    public string DomaineMetierCode { get; private set; } = null!;
    public string Localisation { get; private set; } = null!;
    public Guid EntrepriseId { get; private set; }
    public bool EstSupprimee { get; private set; }

    private AnnonceEmploi()
    {
    }

    public static AnnonceEmploi Publier(
        string titre,
        string description,
        TypeContrat typeContrat,
        string domaineMetierCode,
        string localisation,
        Guid entrepriseId)
    {
        return new AnnonceEmploi
        {
            Titre = titre,
            Description = description,
            TypeContrat = typeContrat,
            DomaineMetierCode = domaineMetierCode,
            Localisation = localisation,
            EntrepriseId = entrepriseId,
            EstSupprimee = false,
        };
    }

    public void Moderer()
    {
        EstSupprimee = true;
    }
}
