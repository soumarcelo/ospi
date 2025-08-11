using Engine.Domain.ValueObjects;

namespace Engine.Application.Interfaces;

public interface IAuthorizationService
{
    //public Task<bool> HasPermissionAsync(Guid userId, string permission);
    public Task<IList<AuthorizationPermission>> GetUserPermissions(Guid userId);
}
