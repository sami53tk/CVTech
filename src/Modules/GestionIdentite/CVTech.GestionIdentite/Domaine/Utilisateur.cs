using CVTech.BuildingBlocks.Domaine;
using CVTech.BuildingBlocks.Permissions;
using CVTech.GestionIdentite.Domaine.ValueObjects;

namespace CVTech.GestionIdentite.Domaine;

/// <summary>
/// Agrégat racine commun aux trois typologies d'utilisateurs de la plateforme.
/// </summary>
public abstract class Utilisateur : AgregatRacine
{
    public Email Email { get; private set; } = null!;

    public MotDePasseHache MotDePasse { get; private set; } = null!;

    public RoleUtilisateur Role { get; protected init; }

    public bool EstBloque { get; private set; }

    protected Utilisateur()
    {
    }

    protected Utilisateur(Email email, MotDePasseHache motDePasse, RoleUtilisateur role)
    {
        Email = email;
        MotDePasse = motDePasse;
        Role = role;
        EstBloque = false;
    }

    public void Bloquer() => EstBloque = true;

    public void Reactiver() => EstBloque = false;

    public void ChangerMotDePasse(MotDePasseHache nouveauMotDePasse) => MotDePasse = nouveauMotDePasse;
}

public sealed class ProfilCandidat : Utilisateur
{
    public string Nom { get; private set; } = string.Empty;

    public string Prenom { get; private set; } = string.Empty;

    private ProfilCandidat()
    {
    }

    private ProfilCandidat(Email email, MotDePasseHache motDePasse, string nom, string prenom)
        : base(email, motDePasse, RoleUtilisateur.Candidat)
    {
        Nom = nom;
        Prenom = prenom;
    }

    public static ProfilCandidat Inscrire(Email email, MotDePasseHache motDePasse, string nom, string prenom)
        => new(email, motDePasse, nom, prenom);
}

public sealed class ProfilEntreprise : Utilisateur
{
    public string RaisonSociale { get; private set; } = string.Empty;

    private ProfilEntreprise()
    {
    }

    private ProfilEntreprise(Email email, MotDePasseHache motDePasse, string raisonSociale)
        : base(email, motDePasse, RoleUtilisateur.Entreprise)
    {
        RaisonSociale = raisonSociale;
    }

    public static ProfilEntreprise Inscrire(Email email, MotDePasseHache motDePasse, string raisonSociale)
        => new(email, motDePasse, raisonSociale);
}

public sealed class Administrateur : Utilisateur
{
    public string Nom { get; private set; } = string.Empty;

    private Administrateur()
    {
    }

    private Administrateur(Email email, MotDePasseHache motDePasse, string nom)
        : base(email, motDePasse, RoleUtilisateur.Administrateur)
    {
        Nom = nom;
    }

    public static Administrateur Creer(Email email, MotDePasseHache motDePasse, string nom)
        => new(email, motDePasse, nom);
}
