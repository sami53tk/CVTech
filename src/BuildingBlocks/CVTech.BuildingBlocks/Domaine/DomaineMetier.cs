namespace CVTech.BuildingBlocks.Domaine;

/// <summary>
/// Value Object partagé représentant un domaine métier (ex: "cloud-azure", "data-science").
/// Utilisé par CatalogueEmploi, AppelOffreFreelance et ActualiteEtAbonnement pour catégoriser
/// annonces, appels d'offres, articles et abonnements.
/// </summary>
public sealed record DomaineMetier
{
    public string Code { get; }
    public string Libelle { get; }

    public DomaineMetier(string code, string libelle)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException("Le code du domaine métier est obligatoire.", nameof(code));
        }

        if (string.IsNullOrWhiteSpace(libelle))
        {
            throw new ArgumentException("Le libellé du domaine métier est obligatoire.", nameof(libelle));
        }

        Code = code.Trim().ToLowerInvariant();
        Libelle = libelle.Trim();
    }
}

/// <summary>
/// Contrat public exposé par le module ActualiteEtAbonnement (propriétaire du référentiel)
/// et consommé par CatalogueEmploi et AppelOffreFreelance pour valider/lister les domaines métier.
/// </summary>
public interface IReferentielDomaineMetier
{
    Task<IReadOnlyCollection<DomaineMetier>> ListerAsync(CancellationToken ct = default);

    Task<bool> ExisteAsync(string code, CancellationToken ct = default);
}
