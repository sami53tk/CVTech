using CVTech.CatalogueEmploi.Domaine;

namespace CVTech.CatalogueEmploi.Client.Dto;

public sealed record PublierAnnonceRequete(
    string Titre,
    string Description,
    TypeContrat TypeContrat,
    string DomaineMetierCode,
    string Localisation
);

public sealed record CandidatureRequete(string? Message);

public sealed record CvRequete(string Titre, string Resume, IReadOnlyCollection<string> Competences);
