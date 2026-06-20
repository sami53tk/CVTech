using CVTech.BuildingBlocks.Exceptions;
using CVTech.GestionIdentite.Domaine;
using CVTech.GestionIdentite.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CVTech.GestionIdentite.Application.Features.ConsulterProfil;

public sealed class ConsulterProfilHandler : IRequestHandler<ConsulterProfilQuery, ProfilResultat>
{
    private readonly IdentiteDbContext _dbContext;

    public ConsulterProfilHandler(IdentiteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ProfilResultat> Handle(ConsulterProfilQuery request, CancellationToken cancellationToken)
    {
        var utilisateur = await _dbContext.Utilisateurs
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == request.UtilisateurId, cancellationToken)
            ?? throw new EntiteIntrouvableException("Utilisateur", request.UtilisateurId);

        return utilisateur switch
        {
            ProfilCandidat candidat => new ProfilResultat(
                candidat.Id, candidat.Email.Valeur, candidat.Role, candidat.EstBloque,
                candidat.Nom, candidat.Prenom, null),

            ProfilEntreprise entreprise => new ProfilResultat(
                entreprise.Id, entreprise.Email.Valeur, entreprise.Role, entreprise.EstBloque,
                null, null, entreprise.RaisonSociale),

            Administrateur admin => new ProfilResultat(
                admin.Id, admin.Email.Valeur, admin.Role, admin.EstBloque,
                admin.Nom, null, null),

            _ => throw new InvalidOperationException("Type d'utilisateur inconnu."),
        };
    }
}
