using CVTech.BuildingBlocks.Permissions;
using MediatR;

namespace CVTech.GestionIdentite.Application.Features.InscrireUtilisateur;

public sealed record InscrireUtilisateurCommand(
    string Email,
    string MotDePasse,
    RoleUtilisateur Role,
    string? Nom,
    string? Prenom,
    string? RaisonSociale
) : IRequest<InscrireUtilisateurResultat>;

public sealed record InscrireUtilisateurResultat(Guid UtilisateurId, RoleUtilisateur Role, string Jeton);
