using Confluent.Kafka;
using Infrastructure.Driven.Outbox;
using Infrastructure.Driven.Repositories;
using Medallion.Threading.Postgres;
using Quartz;

namespace Infrastructure.Driven.Jobs;

[DisallowConcurrentExecution]
public class OutboxMessageProcessorJob : IJob
{
    private readonly OutboxRepository _outboxRepository;
    private readonly PostgresDistributedLock _postgresDistributedLock;

    public OutboxMessageProcessorJob(OutboxRepository outboxRepository, PostgresDistributedLock postgresDistributedLock)
    {
        _outboxRepository = outboxRepository;
        _postgresDistributedLock = postgresDistributedLock;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        await using (await _postgresDistributedLock.AcquireAsync())
        {
            Console.WriteLine("Start outbox job at:" + DateTime.UtcNow.ToLongTimeString());

            var pendingMessages = await _outboxRepository.GetPendingMessagesToProcessAsync(CancellationToken.None);

            var outboxMessages = pendingMessages as OutboxMessage[] ?? pendingMessages.ToArray();
            if (!outboxMessages.Any())
            {
                return;
            }
            
            var config = KafkaProducerConfiguration.GetConfig();
            using var producer = new ProducerBuilder<Null, string>(config).Build();

            foreach (var outboxMessage in outboxMessages.OrderBy(x => x.OccurredOnUtc))
            {
                var deliveryReport = 
                    await producer.ProduceAsync("my-topic", new Message<Null, string> { Value = outboxMessage.Content });
                if (deliveryReport.Status == PersistenceStatus.Persisted)
                {
                    outboxMessage.ProcessedOnUtc = DateTime.UtcNow;
                    Console.WriteLine($"Produced message to {deliveryReport.Topic} partition {deliveryReport.Partition} @ offset {deliveryReport.Offset}");
                }
            }
            
            await _outboxRepository.MarkEventsAsProcessedAsync(
                outboxMessages
                    .Where(x => x.ProcessedOnUtc != null)
                    .Select(x => x.Id)
                    .ToList());
            
            await Task.Delay(500);
        }
    }
}