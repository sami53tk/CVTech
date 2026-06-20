using CVTech.BuildingBlocks.Exceptions;
using CVTech.GestionIdentite.Domaine.Exceptions;
using CVTech.GestionIdentite.Domaine.ValueObjects;
using CVTech.GestionIdentite.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CVTech.GestionIdentite.Application.Features.SeConnecter;

public sealed class SeConnecterHandler : IRequestHandler<SeConnecterCommand, SeConnecterResultat>
{
    private readonly IdentiteDbContext _dbContext;
    private readonly IHacheurMotDePasse _hacheurMotDePasse;
    private readonly IGenerateurJeton _generateurJeton;

    public SeConnecterHandler(
        IdentiteDbContext dbContext,
        IHacheurMotDePasse hacheurMotDePasse,
        IGenerateurJeton generateurJeton)
    {
        _dbContext = dbContext;
        _hacheurMotDePasse = hacheurMotDePasse;
        _generateurJeton = generateurJeton;
    }

    public async Task<SeConnecterResultat> Handle(SeConnecterCommand request, CancellationToken cancellationToken)
    {
        var email = new Email(request.Email);

        var utilisateur = await _dbContext.Utilisateurs.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

        if (utilisateur is null || !_hacheurMotDePasse.Verifier(request.MotDePasse, utilisateur.MotDePasse))
        {
            throw new IdentifiantsInvalidesException();
        }

        if (utilisateur.EstBloque)
        {
            throw new CompteBloqueException();
        }

        var jeton = _generateurJeton.GenererJeton(utilisateur);

        return new SeConnecterResultat(utilisateur.Id, utilisateur.Role, jeton);
    }
}
