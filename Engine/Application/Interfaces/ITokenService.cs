using Engine.Application.DTOs.AuthCredentials;
using Engine.Domain.Entities;

namespace Engine.Application.Interfaces;

public interface ITokenService
{
    /// <summary>
    /// Generates a JWT token for the specified user ID.
    /// </summary>
    /// <param name="userId">The ID of the user for whom the token is generated.</param>
    /// <returns>A string representing the JWT token.</returns>
    public AuthTokenDTO GenerateToken(User user, AuthCredential credential);

    /// <summary>
    /// Validates the provided JWT token.
    /// </summary>
    /// <param name="token">The JWT token to validate.</param>
    /// <returns>True if the token is valid; otherwise, false.</returns>
    public bool ValidateToken(string token);

    /// <summary>
    /// Extracts the user ID from the provided JWT token.
    /// </summary>
    /// <param name="token">The JWT token from which to extract the user ID.</param>
    /// <returns>The user ID if extraction is successful; otherwise, null.</returns>
    public Guid? GetUserIdFromToken(string token);
}
