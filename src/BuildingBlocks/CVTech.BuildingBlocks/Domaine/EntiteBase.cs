namespace CVTech.BuildingBlocks.Domaine;

/// <summary>
/// Classe de base pour toute entité du domaine, quel que soit le module.
/// </summary>
public abstract class EntiteBase
{
    public Guid Id { get; protected set; } = Guid.NewGuid();

    public DateTimeOffset DateCreation { get; protected set; } = DateTimeOffset.UtcNow;
}

/// <summary>
/// Marqueur pour les agrégats racines (point d'entrée transactionnel d'un agrégat DDD).
/// </summary>
public abstract class AgregatRacine : EntiteBase
{
}
