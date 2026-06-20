using CVTech.AppelOffreFreelance.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CVTech.AppelOffreFreelance.Application.Features.ConsulterAppelsOffres;

public sealed class ConsulterAppelsOffresHandler : IRequestHandler<ConsulterAppelsOffresQuery, IReadOnlyCollection<AppelOffreResume>>
{
    private readonly AppelOffreFreelanceDbContext _dbContext;

    public ConsulterAppelsOffresHandler(AppelOffreFreelanceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<AppelOffreResume>> Handle(ConsulterAppelsOffresQuery request, CancellationToken cancellationToken)
    {
        var requete = _dbContext.AppelsOffres.Where(a => !a.EstSupprime);

        if (!string.IsNullOrWhiteSpace(request.DomaineMetierCode))
        {
            requete = requete.Where(a => a.DomaineMetierCode == request.DomaineMetierCode);
        }

        var resultats = await requete
            .Select(a => new AppelOffreResume(a.Id, a.Titre, a.DomaineMetierCode, a.Localisation,
                a.BudgetMax, a.EntrepriseId, a.DateCreation, a.LaureatId.HasValue))
            .ToListAsync(cancellationToken);

        // Tri en mémoire : SQLite ne sait pas trier par DateTimeOffset côté serveur.
        return resultats.OrderByDescending(a => a.DatePublication).ToList();
    }
}
