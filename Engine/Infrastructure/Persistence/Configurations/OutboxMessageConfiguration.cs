using Engine.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Engine.Infrastructure.Persistence.Configurations;

public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("OutboxMessages");
        builder.HasKey(om => om.Id);
        builder.Property(om => om.Id).ValueGeneratedOnAdd();

        builder.Property(om => om.AggregateId)
            .IsRequired();

        builder.Property(om => om.MessageType)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(om => om.Data)
            .IsRequired()
            .HasMaxLength(int.MaxValue);

        builder.Property(om => om.Status)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(om => om.Attempts)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(om => om.CreatedAt)
            .IsRequired();

        builder.Property(om => om.OccurredOn)
            .IsRequired();

        builder.Property(om => om.UpdatedAt)
            .IsRequired(false);
    }
}
