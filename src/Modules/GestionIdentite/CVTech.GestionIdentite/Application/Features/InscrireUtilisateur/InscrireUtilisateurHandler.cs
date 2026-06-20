using CVTech.BuildingBlocks.Permissions;
using CVTech.GestionIdentite.Domaine;
using CVTech.GestionIdentite.Domaine.Exceptions;
using CVTech.GestionIdentite.Domaine.ValueObjects;
using CVTech.GestionIdentite.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CVTech.GestionIdentite.Application.Features.InscrireUtilisateur;

public sealed class InscrireUtilisateurHandler
    : IRequestHandler<InscrireUtilisateurCommand, InscrireUtilisateurResultat>
{
    private readonly IdentiteDbContext _dbContext;
    private readonly IHacheurMotDePasse _hacheurMotDePasse;
    private readonly IGenerateurJeton _generateurJeton;

    public InscrireUtilisateurHandler(
        IdentiteDbContext dbContext,
        IHacheurMotDePasse hacheurMotDePasse,
        IGenerateurJeton generateurJeton)
    {
        _dbContext = dbContext;
        _hacheurMotDePasse = hacheurMotDePasse;
        _generateurJeton = generateurJeton;
    }

    public async Task<InscrireUtilisateurResultat> Handle(InscrireUtilisateurCommand request, CancellationToken cancellationToken)
    {
        var email = new Email(request.Email);

        var emailDejaUtilise = await _dbContext.Utilisateurs.AnyAsync(u => u.Email == email, cancellationToken);
        if (emailDejaUtilise)
        {
            throw new EmailDejaUtiliseException(email.Valeur);
        }

        var motDePasseHache = _hacheurMotDePasse.Hacher(request.MotDePasse);

        Utilisateur utilisateur = request.Role switch
        {
            RoleUtilisateur.Candidat => ProfilCandidat.Inscrire(email, motDePasseHache, request.Nom ?? string.Empty, request.Prenom ?? string.Empty),
            RoleUtilisateur.Entreprise => ProfilEntreprise.Inscrire(email, motDePasseHache, request.RaisonSociale ?? string.Empty),
            _ => throw new ArgumentOutOfRangeException(nameof(request.Role), request.Role, "Rôle non inscriptible."),
        };

        _dbContext.Utilisateurs.Add(utilisateur);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var jeton = _generateurJeton.GenererJeton(utilisateur);

        return new InscrireUtilisateurResultat(utilisateur.Id, utilisateur.Role, jeton);
    }
}
