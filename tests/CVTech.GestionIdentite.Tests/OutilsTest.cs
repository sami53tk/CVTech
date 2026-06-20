using CVTech.GestionIdentite.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CVTech.GestionIdentite.Tests;

internal static class OutilsTest
{
    public static IdentiteDbContext CreerContexte()
    {
        var options = new DbContextOptionsBuilder<IdentiteDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new IdentiteDbContext(options);
    }

    public static IHacheurMotDePasse CreerHacheurMotDePasse() => new HacheurMotDePassePbkdf2();

    public static IGenerateurJeton CreerGenerateurJeton() => new GenerateurJetonJwt(Options.Create(new OptionsJwt
    {
        CleSecrete = "Cle-De-Test-Suffisamment-Longue-Pour-Hmac-Sha256-2026",
        Emetteur = "CVTech.Tests",
        Audience = "CVTech.Tests",
        DureeValiditeHeures = 1,
    }));
}
