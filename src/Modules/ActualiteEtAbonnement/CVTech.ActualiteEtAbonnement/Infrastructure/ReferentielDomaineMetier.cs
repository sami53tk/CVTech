using CVTech.BuildingBlocks.Domaine;
using Microsoft.EntityFrameworkCore;

namespace CVTech.ActualiteEtAbonnement.Infrastructure;

/// <summary>
/// Implémentation du contrat public <see cref="IReferentielDomaineMetier"/>, consommé par
/// CatalogueEmploi et AppelOffreFreelance pour valider/lister les domaines métier. Seul ce
/// module (ActualiteEtAbonnement) possède la base de données du référentiel.
/// </summary>
public sealed class ReferentielDomaineMetier : IReferentielDomaineMetier
{
    private readonly ActualiteEtAbonnementDbContext _dbContext;

    public ReferentielDomaineMetier(ActualiteEtAbonnementDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<DomaineMetier>> ListerAsync(CancellationToken ct = default)
    {
        var domaines = await _dbContext.DomainesMetier.OrderBy(d => d.Libelle).ToListAsync(ct);
        return domaines.Select(d => d.VersValueObject()).ToList();
    }

    public Task<bool> ExisteAsync(string code, CancellationToken ct = default)
    {
        var codeNormalise = code.Trim().ToLowerInvariant();
        return _dbContext.DomainesMetier.AnyAsync(d => d.Code == codeNormalise, ct);
    }
}
