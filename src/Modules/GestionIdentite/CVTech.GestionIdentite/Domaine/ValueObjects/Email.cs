using System.Text.RegularExpressions;
using CVTech.GestionIdentite.Domaine.Exceptions;

namespace CVTech.GestionIdentite.Domaine.ValueObjects;

public sealed partial record Email
{
    public string Valeur { get; }

    public Email(string valeur)
    {
        if (string.IsNullOrWhiteSpace(valeur) || !RegexEmail().IsMatch(valeur))
        {
            throw new EmailInvalideException(valeur);
        }

        Valeur = valeur.Trim().ToLowerInvariant();
    }

    public override string ToString() => Valeur;

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    private static partial Regex RegexEmail();
}
