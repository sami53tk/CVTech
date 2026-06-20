using CVTech.BuildingBlocks.Permissions;

namespace CVTech.GestionIdentite.Client.Dto;

public sealed record InscriptionRequete(
    string Email,
    string MotDePasse,
    RoleUtilisateur Role,
    string? Nom,
    string? Prenom,
    string? RaisonSociale
);

public sealed record ConnexionRequete(string Email, string MotDePasse);

public sealed record JetonReponse(Guid UtilisateurId, string Role, string Jeton);
