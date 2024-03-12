namespace Infrastructure.Driven.Outbox;

public record OutboxMessage(Guid Id, string AggregateName, string Type, string Content, DateTime OccurredOnUtc, DateTime? ProcessedOnUtc);
