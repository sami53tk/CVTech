namespace CVTech.BuildingBlocks.Exceptions;

/// <summary>
/// Classe de base de toutes les exceptions métier de la plateforme. Une exception métier
/// représente la violation d'une règle du domaine (et non une erreur technique).
/// </summary>
public abstract class ExceptionMetier : Exception
{
    protected ExceptionMetier(string message) : base(message)
    {
    }
}

/// <summary>
/// Levée par <see cref="CVTech.BuildingBlocks.Permissions.IVerificateurPermission"/> lorsqu'un
/// utilisateur tente une action que son rôle ne l'autorise pas à effectuer.
/// </summary>
public sealed class PermissionRefuseeException : ExceptionMetier
{
    public string Action { get; }

    public PermissionRefuseeException(string action)
        : base($"L'action « {action} » est refusée pour cet utilisateur.")
    {
        Action = action;
    }
}

/// <summary>
/// Levée lorsqu'un compte bloqué par un administrateur tente une action réservée aux comptes actifs
/// (ex : se connecter, postuler, soumettre une proposition).
/// </summary>
public sealed class CompteBloqueException : ExceptionMetier
{
    public CompteBloqueException()
        : base("Ce compte a été bloqué par un administrateur et ne peut pas effectuer cette action.")
    {
    }
}

/// <summary>
/// Levée lorsqu'une entité (annonce, appel d'offre, article...) n'existe pas ou n'est plus accessible.
/// </summary>
public sealed class EntiteIntrouvableException : ExceptionMetier
{
    public EntiteIntrouvableException(string typeEntite, Guid id)
        : base($"{typeEntite} avec l'identifiant « {id} » est introuvable.")
    {
    }
}
