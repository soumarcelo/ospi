using Engine.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Engine.Infrastructure.Persistence.Configurations;

public class AuthCredentialConfiguration : IEntityTypeConfiguration<AuthCredential>
{
    public void Configure(EntityTypeBuilder<AuthCredential> builder)
    {
        builder.ToTable("AuthCredentials");
        builder.HasKey(ac => ac.Id);
        builder.Property(ac => ac.Id).ValueGeneratedOnAdd();

        builder.Property(ac => ac.AuthProvider)
            .IsRequired()
            .HasMaxLength(24)
            .HasDefaultValue("email")
            .HasConversion<string>();

        builder.Property(ac => ac.ProviderKey)
            .IsRequired()
            .HasMaxLength(512);

        builder.Property(ac => ac.SecretHash)
            .IsRequired()
            .HasMaxLength(512);

        builder.Property(ac => ac.IsVerified)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(ac => ac.CreatedAt)
            .IsRequired();

        builder.Property(ac => ac.VerifiedAt)
            .IsRequired(false);

        builder.Property(ac => ac.UpdatedAt)
            .IsRequired(false);
    }
}
