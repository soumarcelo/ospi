using Engine.Application.Common.Results;
using Engine.Application.Requests;
using Engine.Domain.Entities;
using Engine.Domain.ValueObjects;

namespace Engine.Application.Interfaces.Services;

public interface IUserService
{
    public Task<IResult<User>> AddUserAsync(CreateUserRequest user);
    public Task<IResult<User>> GetUserByIdAsync(Guid userId);
    public Task<IResult<User>> GetUserByEmailAsync(EmailAddress email);
    //public Task<IResult<IEnumerable<User>>> GetAllUsersAsync();
    public IResult<Guid> UpdateUser(User user);
}
