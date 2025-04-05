using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Seraphine.Core.Interface;
using Microsoft.IdentityModel.Tokens;

namespace Seraphine.Core;

public class LoginMiddleware(RequestDelegate _next)
{
    private const string SecretKey = @"https://open.spotify.com/intl-pt/track/7FwSZyO5ynlN0OJGVOjE6k?si=a78fc4f731444018";

    public async Task InvokeAsync(HttpContext context)
    {
        var rotasAnonimas = new[] { "/login", "/appVersion" }; // Rotas que não requerem autenticação
        if (rotasAnonimas.Any(route => context.Request.Path.StartsWithSegments(route)))
        {
            await _next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue("Authorization", out var tokenHeader) || string.IsNullOrWhiteSpace(tokenHeader))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Não foi informado 'Authorization Header'.");
            return;
        }

        var token = tokenHeader.ToString().Replace("Bearer ", string.Empty);

        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));

            tokenHandler.ValidateToken(token, new()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "Seraphine",
                ValidAudience = "Seraphine",
                IssuerSigningKey = securityKey
            }, out _);

            if (context.RequestServices.GetService(typeof(ICredentialUser)) is not ICredentialUser credentialUser)
            {
                await context.Response.WriteAsync("Não foi instanciado a ICredencialUser");
                return;
            }

            var jwtToken = tokenHandler.ReadJwtToken(token);
            var claimValue = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            credentialUser.SetarIdUsuarioLogado(Guid.Parse(claimValue ?? string.Empty));

            await _next(context);
        }
        catch (Exception)
        {
            context.Response.StatusCode = 401; // Unauthorized
            await context.Response.WriteAsync("Token expirado!");
        }
    }
}