using CVTech.BuildingBlocks.Domaine;

namespace CVTech.ActualiteEtAbonnement.Domaine;

/// <summary>
/// Entrée du référentiel des domaines métier, géré par les administrateurs et exposé aux
/// autres modules via le contrat public <see cref="IReferentielDomaineMetier"/>.
/// </summary>
public sealed class DomaineMetierReference : AgregatRacine
{
    public string Code { get; private set; } = null!;
    public string Libelle { get; private set; } = null!;

    private DomaineMetierReference()
    {
    }

    public static DomaineMetierReference Creer(string code, string libelle)
    {
        var domaine = new DomaineMetier(code, libelle);

        return new DomaineMetierReference
        {
            Code = domaine.Code,
            Libelle = domaine.Libelle,
        };
    }

    public DomaineMetier VersValueObject() => new(Code, Libelle);
}
