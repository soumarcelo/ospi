using Engine.Application.DTOs.AuthCredentials;
using Engine.Application.Interfaces;
using Engine.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Engine.Infrastructure.Services;

public class JWTTokenService(IConfiguration configuration) : ITokenService
{
    public AuthTokenDTO GenerateToken(User user, AuthCredential credential)
    {
        string? secret = configuration["JwtSettings:Secret"];
        string? issuer = configuration["JwtSettings:Issuer"];
        string? audience = configuration["JwtSettings:Audience"];
        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(secret ?? ""));
        SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, credential.ProviderKey),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Name, user.FullName)
        };

        JwtSecurityToken token = new(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1), // Tempo de expiração do token
            signingCredentials: credentials);

        JwtSecurityTokenHandler tokenHandler = new();
        string jwt = tokenHandler.WriteToken(token);
        string refreshToken = Guid.NewGuid().ToString("N"); // Placeholder for refresh token logic
        AuthTokenDTO dto = new()
        {
            Token = jwt,
            ExpiresAt = token.ValidTo,
            RefreshToken = refreshToken,
            RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(30)
        };

        // Proximos passos:
        // 1. Persistir o refresh token no banco de dados associado ao usuário
        // 2. Implementar lógica para renovar o token JWT usando o refresh token
        // 3. Implementar lógica para revogar tokens quando necessário

        return dto;
    }

    public bool ValidateToken(string token)
    {
        // Placeholder for JWT validation logic
        // In a real implementation, this would validate the JWT token's signature and claims
        return !string.IsNullOrEmpty(token) && token.StartsWith("token-for-user-");
    }

    public Guid? GetUserIdFromToken(string token)
    {
        // Placeholder for extracting user ID from the JWT token
        if (ValidateToken(token))
        {
            string userIdString = token.Replace("token-for-user-", "");
            if (Guid.TryParse(userIdString, out Guid userId))
            {
                return userId;
            }
        }
        return null;
    }
}
