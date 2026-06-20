namespace CVTech.GestionIdentite.Domaine.ValueObjects;

/// <summary>
/// Représente un mot de passe déjà haché (sel + empreinte). La production de cette valeur
/// (algorithme de hachage) est une préoccupation technique déléguée à l'Infrastructure
/// (<c>IHacheurMotDePasse</c>) ; le Domaine ne manipule jamais de mot de passe en clair.
/// </summary>
public sealed record MotDePasseHache(string Valeur);
