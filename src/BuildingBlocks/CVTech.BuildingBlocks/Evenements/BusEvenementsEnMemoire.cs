using Microsoft.Extensions.DependencyInjection;

namespace CVTech.BuildingBlocks.Evenements;

/// <summary>
/// Implémentation en mémoire du bus d'événements interne : à la publication d'un événement,
/// résout dans un scope DI dédié tous les <see cref="IGestionnaireEvenement{TEvenement}"/>
/// enregistrés par les modules consommateurs et les exécute séquentiellement.
/// </summary>
public sealed class BusEvenementsEnMemoire : IBusEvenements
{
    private readonly IServiceProvider _fournisseurServices;

    public BusEvenementsEnMemoire(IServiceProvider fournisseurServices)
    {
        _fournisseurServices = fournisseurServices;
    }

    public async Task PublierAsync<TEvenement>(TEvenement evenement, CancellationToken ct = default)
        where TEvenement : IEvenementIntegration
    {
        using var scope = _fournisseurServices.CreateScope();
        var gestionnaires = scope.ServiceProvider.GetServices<IGestionnaireEvenement<TEvenement>>();

        foreach (var gestionnaire in gestionnaires)
        {
            await gestionnaire.GererAsync(evenement, ct);
        }
    }
}
