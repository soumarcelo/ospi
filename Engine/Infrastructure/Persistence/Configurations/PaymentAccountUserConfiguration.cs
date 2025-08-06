using Engine.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Engine.Infrastructure.Persistence.Configurations;

public class PaymentAccountUserConfiguration : IEntityTypeConfiguration<PaymentAccountUser>
{
    public void Configure(EntityTypeBuilder<PaymentAccountUser> builder)
    {
        builder.ToTable("PaymentAccountUsers");
        builder.HasKey(pu => pu.Id);
        builder.Property(pu => pu.Id).ValueGeneratedOnAdd();

        builder.Property(pu => pu.AccountId)
            .IsRequired();

        builder.Property(pu => pu.UserId)
            .IsRequired();

        builder.Property(pu => pu.Role)
            .IsRequired()
            .HasMaxLength(20)
            .HasDefaultValue("Owner")
            .HasConversion<string>();

        builder.Property(pu => pu.CreatedAt)
            .IsRequired();

        builder.Property(pu => pu.UpdatedAt)
            .IsRequired(false);
    }
}
