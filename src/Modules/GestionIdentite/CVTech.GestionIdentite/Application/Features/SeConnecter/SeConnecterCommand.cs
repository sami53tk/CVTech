using CVTech.BuildingBlocks.Permissions;
using MediatR;

namespace CVTech.GestionIdentite.Application.Features.SeConnecter;

public sealed record SeConnecterCommand(string Email, string MotDePasse) : IRequest<SeConnecterResultat>;

public sealed record SeConnecterResultat(Guid UtilisateurId, RoleUtilisateur Role, string Jeton);
