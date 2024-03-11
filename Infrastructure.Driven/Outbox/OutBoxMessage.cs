namespace Infrastructure.Driven.Outbox;

public record OutboxMessage(Guid Id, string Type, string Content, DateTime OccurredOnUtc, DateTime? ProcessedOnUtc, string RoutingKey);
