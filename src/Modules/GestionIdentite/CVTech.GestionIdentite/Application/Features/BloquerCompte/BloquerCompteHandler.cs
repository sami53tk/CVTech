using CVTech.BuildingBlocks.Exceptions;
using CVTech.BuildingBlocks.Permissions;
using CVTech.GestionIdentite.Infrastructure;
using MediatR;

namespace CVTech.GestionIdentite.Application.Features.BloquerCompte;

public sealed class BloquerCompteHandler : IRequestHandler<BloquerCompteCommand>
{
    private readonly IdentiteDbContext _dbContext;
    private readonly IVerificateurPermission _verificateurPermission;

    public BloquerCompteHandler(IdentiteDbContext dbContext, IVerificateurPermission verificateurPermission)
    {
        _dbContext = dbContext;
        _verificateurPermission = verificateurPermission;
    }

    public async Task Handle(BloquerCompteCommand request, CancellationToken cancellationToken)
    {
        _verificateurPermission.VerifierPermission(request.RoleActeur, Permissions.BloquerReactiverCompte);

        var utilisateur = await _dbContext.Utilisateurs.FindAsync([request.CompteId], cancellationToken)
            ?? throw new EntiteIntrouvableException("Utilisateur", request.CompteId);

        utilisateur.Bloquer();
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
