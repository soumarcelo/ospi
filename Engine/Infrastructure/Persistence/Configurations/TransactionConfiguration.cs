using Engine.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Engine.Infrastructure.Persistence.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("Transactions");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id)
            .ValueGeneratedOnAdd();

        builder.Property(t => t.InitiatorUserId)
            .IsRequired(false);

        builder.Property(t => t.ReversalOfTransactionId)
            .IsRequired(false);

        builder.Property(t => t.FromAccountId)
            .IsRequired(false);

        builder.Property(t => t.ToAccountId)
            .IsRequired(false);

        builder.Property(t => t.ExternalReferenceType)
            .IsRequired(false)
            .HasMaxLength(5)
            .HasConversion<string>();

        builder.Property(t => t.ExternalReference)
            .IsRequired(false)
            .HasMaxLength(100);

        builder.OwnsOne(t => t.Amount, amountBuilder =>
        {
            amountBuilder.Property(a => a.Value)
                .IsRequired()
                .HasColumnName("AmountValue");

            amountBuilder.Property(a => a.Currency)
                .IsRequired()
                .HasColumnName("AmountCurrency")
                .HasMaxLength(5)
                .HasDefaultValue("BRL");
        });

        builder.OwnsOne(t => t.FeeAmount, feeBuilder =>
        {
            feeBuilder.Property(f => f.Value)
                .IsRequired()
                .HasColumnName("FeeAmountValue");

            feeBuilder.Property(f => f.Currency)
                .IsRequired()
                .HasColumnName("FeeAmountCurrency")
                .HasMaxLength(5)
                .HasDefaultValue("BRL");
        });

        builder.Property(t => t.Status)
            .IsRequired()
            .HasMaxLength(20)
            .HasDefaultValue("Pending")
            .HasConversion<string>();

        builder.Property(t => t.FailureReasonCode)
            .IsRequired(false)
            .HasMaxLength(16)
            .HasConversion<string>();

        builder.Property(t => t.FailureReasonDescription)
            .IsRequired(false)
            .HasMaxLength(500);

        builder.Property(t => t.Direction)
            .IsRequired()
            .HasMaxLength(20)
            .HasConversion<string>();

        builder.Property(t => t.TransactionType)
            .IsRequired()
            .HasMaxLength(20)
            .HasConversion<string>();

        builder.Property(t => t.MethodType)
            .IsRequired()
            .HasMaxLength(20)
            .HasConversion<string>();

        builder.Property(t => t.IsExternal)
            .IsRequired();

        builder.Property(t => t.TransactionDetails)
            .IsRequired(false)
            .HasColumnType("jsonb");

        builder.Property(t => t.Description)
            .IsRequired(false);

        builder.Property(t => t.CreatedAt)
            .IsRequired();

        builder.Property(t => t.CompletedAt)
            .IsRequired(false);

        builder.Property(t => t.FailedAt)
            .IsRequired(false);

        builder.Property(t => t.CancelledAt)
            .IsRequired(false);

        builder.Property(t => t.ReversedAt)
            .IsRequired(false);

        builder.Property(t => t.UpdatedAt)
            .IsRequired(false);
    }
}
