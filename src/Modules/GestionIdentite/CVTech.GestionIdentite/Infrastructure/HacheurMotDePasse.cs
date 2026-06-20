using System.Security.Cryptography;
using CVTech.GestionIdentite.Domaine.ValueObjects;

namespace CVTech.GestionIdentite.Infrastructure;

public interface IHacheurMotDePasse
{
    MotDePasseHache Hacher(string motDePasseEnClair);

    bool Verifier(string motDePasseEnClair, MotDePasseHache hache);
}

public sealed class HacheurMotDePassePbkdf2 : IHacheurMotDePasse
{
    private const int TailleSelOctets = 16;
    private const int TailleHacheOctets = 32;
    private const int Iterations = 100_000;
    private static readonly HashAlgorithmName Algorithme = HashAlgorithmName.SHA256;

    public MotDePasseHache Hacher(string motDePasseEnClair)
    {
        var sel = RandomNumberGenerator.GetBytes(TailleSelOctets);
        var hache = Rfc2898DeriveBytes.Pbkdf2(motDePasseEnClair, sel, Iterations, Algorithme, TailleHacheOctets);
        return new MotDePasseHache($"{Convert.ToBase64String(sel)}.{Convert.ToBase64String(hache)}");
    }

    public bool Verifier(string motDePasseEnClair, MotDePasseHache hache)
    {
        var parties = hache.Valeur.Split('.');
        if (parties.Length != 2)
        {
            return false;
        }

        var sel = Convert.FromBase64String(parties[0]);
        var hacheAttendu = Convert.FromBase64String(parties[1]);
        var hacheCalcule = Rfc2898DeriveBytes.Pbkdf2(motDePasseEnClair, sel, Iterations, Algorithme, hacheAttendu.Length);

        return CryptographicOperations.FixedTimeEquals(hacheCalcule, hacheAttendu);
    }
}
