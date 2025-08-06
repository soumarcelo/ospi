using Engine.Application.Interfaces.Repositories;
using Engine.Domain.Entities;
using Engine.Domain.Enums;
using Engine.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Engine.Infrastructure.Persistence.Repositories;

public class AuthCredentialRepository(AppDbContext _context) : IAuthCredentialRepository
{
    public async Task<AuthCredential?> GetByIdAsync(Guid id)
    {
        return await _context.AuthCredentials.FindAsync(id);
    }

    public async Task<AuthCredential?> GetByUserIdAsync(Guid userId, AuthCredentialProvider provider)
    {
        return await _context.AuthCredentials
            .FirstOrDefaultAsync(ac => ac.UserId == userId &&
                ac.AuthProvider == provider);
    }

    public async Task<AuthCredential?> GetByEmailAsync(EmailAddress email)
    {
        return await _context.AuthCredentials
            .FirstOrDefaultAsync(ac => 
                ac.AuthProvider == AuthCredentialProvider.Email &&
                ac.ProviderKey == email.Value);
    }

    public async Task AddAsync(AuthCredential authCredential)
    {
        await _context.AuthCredentials.AddAsync(authCredential);
    }

    public void Update(AuthCredential authCredential)
    {
        _context.AuthCredentials.Update(authCredential);
    }

    public async Task DeleteAsync(Guid authCredentialId)
    {
        AuthCredential? authCredential = await GetByIdAsync(authCredentialId) ??
            throw new KeyNotFoundException($"Auth credential with ID {authCredentialId} not found.");
        _context.AuthCredentials.Remove(authCredential);
    }
}
