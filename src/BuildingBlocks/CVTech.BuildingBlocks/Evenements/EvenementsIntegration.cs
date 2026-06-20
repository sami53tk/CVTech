namespace CVTech.BuildingBlocks.Evenements;

/// <summary>
/// Publié par le module CatalogueEmploi lorsqu'une entreprise publie une nouvelle annonce d'emploi.
/// Consommé par ActualiteEtAbonnement pour notifier les abonnés du domaine métier concerné.
/// </summary>
public sealed record AnnoncePublieeEvent(
    Guid AnnonceId,
    string Titre,
    string DomaineMetierCode,
    Guid EntrepriseId,
    DateTimeOffset DatePublication
) : IEvenementIntegration;

/// <summary>
/// Publié par le module AppelOffreFreelance lorsqu'une entreprise publie un nouvel appel d'offre.
/// Consommé par ActualiteEtAbonnement pour notifier les abonnés du domaine métier concerné.
/// </summary>
public sealed record AppelOffrePublieEvent(
    Guid AppelOffreId,
    string Titre,
    string DomaineMetierCode,
    Guid EntrepriseId,
    DateTimeOffset DatePublication
) : IEvenementIntegration;
