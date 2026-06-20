namespace CVTech.BuildingBlocks.Evenements;

/// <summary>
/// Marqueur pour tout événement d'intégration transitant par le bus interne en mémoire.
/// </summary>
public interface IEvenementIntegration
{
}

/// <summary>
/// Bus d'événements interne en mémoire. C'est l'unique canal autorisé pour qu'un module
/// notifie les autres modules d'un fait du domaine (ex : AnnoncePubliee, AppelOffrePublie).
/// </summary>
public interface IBusEvenements
{
    Task PublierAsync<TEvenement>(TEvenement evenement, CancellationToken ct = default)
        where TEvenement : IEvenementIntegration;
}

/// <summary>
/// Implémentée par chaque module qui souhaite réagir à un événement publié par un autre module.
/// Enregistrée en DI par le ModuleLoader du module consommateur.
/// </summary>
public interface IGestionnaireEvenement<in TEvenement> where TEvenement : IEvenementIntegration
{
    Task GererAsync(TEvenement evenement, CancellationToken ct = default);
}
