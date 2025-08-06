using Engine.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Engine.Infrastructure.Persistence.Configurations;

public class PaymentAccountConfiguration : IEntityTypeConfiguration<PaymentAccount>
{
    public void Configure(EntityTypeBuilder<PaymentAccount> builder)
    {
        builder.ToTable("PaymentAccounts");
        builder.HasKey(pa => pa.Id);
        builder.Property(pa => pa.Id).ValueGeneratedOnAdd();

        builder.Property(pa => pa.AccountType)
            .IsRequired()
            .HasMaxLength(2)
            .HasDefaultValue("Individual")
            .HasConversion<string>();

        builder.Property(pa => pa.LegalName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(pa => pa.Document)
            .IsRequired()
            .HasMaxLength(14);

        builder.Property(pa => pa.Status)
            .IsRequired()
            .HasMaxLength(24)
            .HasDefaultValue("Active")
            .HasConversion<string>();

        builder.Property(pa => pa.CreatedAt)
            .IsRequired();

        builder.Property(pa => pa.SuspendedAt)
            .IsRequired(false);

        builder.Property(pa => pa.ClosedAt)
            .IsRequired(false);

        builder.Property(pa => pa.UpdatedAt)
            .IsRequired(false);
    }
}
