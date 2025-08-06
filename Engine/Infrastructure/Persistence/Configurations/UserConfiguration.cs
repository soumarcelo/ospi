using Engine.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Engine.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id)
            .ValueGeneratedOnAdd();

        builder.Property(u => u.FullName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.ContactEmail)
            .IsRequired()
            .HasMaxLength(256)
            .IsUnicode(false)
            .HasConversion<string>();

        builder.Property(u => u.Document)
            .IsRequired()
            .HasMaxLength(11)
            .HasConversion<string>();

        builder.Property(u => u.Status)
            .IsRequired()
            .HasMaxLength(24)
            .HasDefaultValue("Active")
            .HasConversion<string>();

        builder.Property(u => u.CreatedAt)
            .IsRequired();

        builder.Property(u => u.UpdatedAt)
            .IsRequired(false);
    }
}
