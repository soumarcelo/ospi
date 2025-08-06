using Engine.Application.Common.Results;
using Engine.Domain.Entities;

namespace Engine.Application.Interfaces.Services;

public interface IPasswordService<TUser>
{
    public IResult<string> HashPassword(TUser user, string password);
    public IResult<bool> VerifyPassword(TUser user, string password, string hashedPassword);
}
