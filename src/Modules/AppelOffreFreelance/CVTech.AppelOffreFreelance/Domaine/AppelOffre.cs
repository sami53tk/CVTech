using CVTech.BuildingBlocks.Domaine;

namespace CVTech.AppelOffreFreelance.Domaine;

public sealed class AppelOffre : AgregatRacine
{
    public string Titre { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public string DomaineMetierCode { get; private set; } = null!;
    public string Localisation { get; private set; } = null!;
    public decimal BudgetMax { get; private set; }
    public Guid EntrepriseId { get; private set; }
    public bool EstSupprime { get; private set; }
    public Guid? LaureatId { get; private set; }

    private AppelOffre() { }

    public static AppelOffre Publier(string titre, string description, string domaineMetierCode,
        string localisation, decimal budgetMax, Guid entrepriseId)
    {
        return new AppelOffre
        {
            Id = Guid.NewGuid(),
            Titre = titre,
            Description = description,
            DomaineMetierCode = domaineMetierCode,
            Localisation = localisation,
            BudgetMax = budgetMax,
            EntrepriseId = entrepriseId,
            EstSupprime = false,
            LaureatId = null,
            DateCreation = DateTimeOffset.UtcNow,
        };
    }

    public void Moderer() => EstSupprime = true;

    public void SelectionnerLaureat(Guid propositionId) => LaureatId = propositionId;
}
