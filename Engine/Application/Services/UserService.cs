using Engine.Application.Common.Results;
using Engine.Application.Interfaces;
using Engine.Application.Interfaces.Repositories;
using Engine.Application.Interfaces.Services;
using Engine.Application.Requests;
using Engine.Domain.Entities;
using Engine.Domain.ValueObjects;

namespace Engine.Application.Services;

public class UserService(IUserRepository repository) : IUserService
{
    public async Task<IResult<User>> AddUserAsync(CreateUserRequest request)
    {
        try
        {
            User user = User.Create(
                request.FullName,
                request.ContactEmail,
                request.Document);
            await repository.AddAsync(user);
            return Result.Success(user);
        }
        catch (Exception ex)
        {
            return Result.Failure<User>($"An error occurred while creating the user: {ex.Message}");
        }
    }

    public async Task<IResult<User>> GetUserByIdAsync(Guid userId)
    {
        try
        {
            User? user = await repository.GetByIdAsync(userId);
            if (user is null)
            {
                return Result.Failure<User>("User not found.");
            }
            return Result.Success(user);
        }
        catch (Exception ex)
        {
            return Result.Failure<User>($"An error occurred while retrieving the user: {ex.Message}");
        }
    }

    public async Task<IResult<User>> GetUserByEmailAsync(EmailAddress email)
    {
        try
        {
            User? user = await repository.GetByEmailAsync(email);
            if (user is null)
            {
                return Result.Failure<User>("User not found.");
            }
            return Result.Success(user);
        }
        catch (Exception ex)
        {
            return Result.Failure<User>($"An error occurred while retrieving the user: {ex.Message}");
        }
    }

    public IResult<Guid> UpdateUser(User user)
    {
        if (user is null)
        {
            return Result.Failure<Guid>("User cannot be null.");
        }

        try
        {
            repository.Update(user);
            return Result.Success(user.Id);
        }
        catch (Exception ex)
        {
            return Result.Failure<Guid>($"An error occurred while updating the user: {ex.Message}");
        }
    }
}
