using Infrastructure.Driven.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Driven.Configurations;

internal sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("outbox_messages");

        builder.HasKey(x => x.Id);
        
        builder.HasIndex(x => x.ProcessedOnUtc)
            .HasFilter(""" "ProcessedOnUtc" IS NULL""")
            .HasDatabaseName("IX_outbox_messages_processed_on_utc_null");
    }
}
