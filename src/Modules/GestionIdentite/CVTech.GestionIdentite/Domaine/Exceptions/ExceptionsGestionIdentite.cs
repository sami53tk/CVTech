using CVTech.BuildingBlocks.Exceptions;

namespace CVTech.GestionIdentite.Domaine.Exceptions;

public sealed class EmailInvalideException : ExceptionMetier
{
    public EmailInvalideException(string valeur)
        : base($"L'adresse e-mail « {valeur} » est invalide.")
    {
    }
}

public sealed class EmailDejaUtiliseException : ExceptionMetier
{
    public EmailDejaUtiliseException(string email)
        : base($"Un compte existe déjà avec l'adresse e-mail « {email} ».")
    {
    }
}

public sealed class IdentifiantsInvalidesException : ExceptionMetier
{
    public IdentifiantsInvalidesException()
        : base("L'adresse e-mail ou le mot de passe est incorrect.")
    {
    }
}
