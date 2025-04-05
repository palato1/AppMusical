using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Seraphine.Helpers;

public static class JwtHelper
{
    public static string GerarTokenJwt(string usuario, Guid idUsuario)
    {
        const string secretKey = @"https://open.spotify.com/intl-pt/track/7FwSZyO5ynlN0OJGVOjE6k?si=a78fc4f731444018";

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, idUsuario.ToString()),
            new Claim(ClaimTypes.Name, usuario),
            new Claim(ClaimTypes.Role, "User")
        };

        var token = new JwtSecurityToken("Seraphine",
            "Seraphine",
            claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}