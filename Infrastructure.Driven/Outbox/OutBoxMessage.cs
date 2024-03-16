namespace Infrastructure.Driven.Outbox;

public class OutboxMessage
{
    public Guid Id { get; set; }
    public string Type { get; set; }
    public string Content { get; set; }
    public DateTime OccurredOnUtc { get; set; }
    public DateTime? ProcessedOnUtc { get; set; }
    public string AggregateName { get; set; }

    public OutboxMessage() { }

    public OutboxMessage(Guid id, string aggregateName, string type, string content, DateTime occurredOnUtc, DateTime? processedOnUtc)
    {
        Id = id;
        Type = type;
        Content = content;
        OccurredOnUtc = occurredOnUtc;
        ProcessedOnUtc = processedOnUtc;
        AggregateName = aggregateName;
    }
}