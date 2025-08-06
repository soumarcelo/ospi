using Engine.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Engine.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<AuthCredential> AuthCredentials { get; set; }
    public DbSet<PaymentAccount> PaymentAccounts { get; set; }
    public DbSet<PaymentAccountUser> UserAccounts { get; set; }
    public DbSet<PaymentMethod> PaymentMethods { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
