using CVTech.BuildingBlocks.Permissions;
using MediatR;

namespace CVTech.GestionIdentite.Application.Features.ConsulterProfil;

public sealed record ConsulterProfilQuery(Guid UtilisateurId) : IRequest<ProfilResultat>;

public sealed record ProfilResultat(
    Guid Id,
    string Email,
    RoleUtilisateur Role,
    bool EstBloque,
    string? Nom,
    string? Prenom,
    string? RaisonSociale
);
