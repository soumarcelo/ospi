using Engine.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Engine.Infrastructure.Persistence.Configurations;

public class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
{
    public void Configure(EntityTypeBuilder<PaymentMethod> builder)
    {
        builder.ToTable("PaymentMethods");
        builder.HasKey(pm => pm.Id);
        builder.Property(pm => pm.Id).ValueGeneratedOnAdd();

        builder.Property(pm => pm.AccountId)
            .IsRequired();

        builder.Property(pm => pm.MethodType)
            .IsRequired()
            .HasMaxLength(50)
            .HasConversion<string>();

        builder.Property(pm => pm.MethodDetails)
            .IsRequired()
            .HasColumnType("jsonb");

        builder.Property(pm => pm.Status)
            .IsRequired()
            .HasDefaultValue("Active")
            .HasConversion<string>();

        builder.Property(pm => pm.CreatedAt)
            .IsRequired();

        builder.Property(pm => pm.UpdatedAt)
            .IsRequired(false);
    }
}
