using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CVTech.GestionIdentite.Domaine;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CVTech.GestionIdentite.Infrastructure;

public sealed class OptionsJwt
{
    public const string Section = "Jwt";

    public string CleSecrete { get; set; } = string.Empty;

    public string Emetteur { get; set; } = "CVTech";

    public string Audience { get; set; } = "CVTech";

    public int DureeValiditeHeures { get; set; } = 8;
}

public interface IGenerateurJeton
{
    string GenererJeton(Utilisateur utilisateur);
}

public sealed class GenerateurJetonJwt : IGenerateurJeton
{
    private readonly OptionsJwt _options;

    public GenerateurJetonJwt(IOptions<OptionsJwt> options)
    {
        _options = options.Value;
    }

    public string GenererJeton(Utilisateur utilisateur)
    {
        var identifiantsSignature = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.CleSecrete)),
            SecurityAlgorithms.HmacSha256);

        var revendications = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, utilisateur.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, utilisateur.Email.Valeur),
            new Claim(ClaimTypes.Role, utilisateur.Role.ToString()),
        };

        var jeton = new JwtSecurityToken(
            issuer: _options.Emetteur,
            audience: _options.Audience,
            claims: revendications,
            expires: DateTime.UtcNow.AddHours(_options.DureeValiditeHeures),
            signingCredentials: identifiantsSignature);

        return new JwtSecurityTokenHandler().WriteToken(jeton);
    }
}
