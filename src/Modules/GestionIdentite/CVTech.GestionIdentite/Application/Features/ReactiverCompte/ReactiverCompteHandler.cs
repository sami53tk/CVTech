using CVTech.BuildingBlocks.Exceptions;
using CVTech.BuildingBlocks.Permissions;
using CVTech.GestionIdentite.Infrastructure;
using MediatR;

namespace CVTech.GestionIdentite.Application.Features.ReactiverCompte;

public sealed class ReactiverCompteHandler : IRequestHandler<ReactiverCompteCommand>
{
    private readonly IdentiteDbContext _dbContext;
    private readonly IVerificateurPermission _verificateurPermission;

    public ReactiverCompteHandler(IdentiteDbContext dbContext, IVerificateurPermission verificateurPermission)
    {
        _dbContext = dbContext;
        _verificateurPermission = verificateurPermission;
    }

    public async Task Handle(ReactiverCompteCommand request, CancellationToken cancellationToken)
    {
        _verificateurPermission.VerifierPermission(request.RoleActeur, Permissions.BloquerReactiverCompte);

        var utilisateur = await _dbContext.Utilisateurs.FindAsync([request.CompteId], cancellationToken)
            ?? throw new EntiteIntrouvableException("Utilisateur", request.CompteId);

        utilisateur.Reactiver();
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
