using Engine.Application.Interfaces.Repositories;
using Engine.Domain.Entities;
using Engine.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Engine.Infrastructure.Persistence.Repositories;

public class UserRepository(AppDbContext _context) : IUserRepository
{
    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User?> GetByEmailAsync(EmailAddress email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.ContactEmail == email);
    }

    public async Task<User?> GetByDocumentAsync(IndividualDocument document)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Document == document);
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
    }

    public void Update(User user)
    {
        _context.Users.Update(user);
    }

    public async Task DeleteAsync(Guid userId)
    {
        User? user = await GetByIdAsync(userId) ??
            throw new KeyNotFoundException($"User with ID {userId} not found.");
        _context.Users.Remove(user);
    }
}
