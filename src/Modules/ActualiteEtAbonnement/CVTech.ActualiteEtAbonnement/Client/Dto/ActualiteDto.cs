namespace CVTech.ActualiteEtAbonnement.Client.Dto;

public sealed record PublierArticleRequete(
    string Titre,
    string Contenu,
    string? DomaineMetierCode,
    string LienExterne);

public sealed record SAbonnerRequete(string DomaineMetierCode);

public sealed record CreerDomaineRequete(string Code, string Libelle);
