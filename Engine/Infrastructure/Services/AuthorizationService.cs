using Engine.Application.Interfaces;
using Engine.Domain.ValueObjects;

namespace Engine.Infrastructure.Services;

public class AuthorizationService : IAuthorizationService
{
    public Task<IList<AuthorizationPermission>> GetUserPermissions(Guid userId)
    {
        IList<AuthorizationPermission> permissions =
        [
            new("PaymentAccountBalance.Read"),
            new("PaymentAccountStatement.Read"),
        ];

        return Task.FromResult(permissions);
    }
}
