using System.Data;
using Infrastructure.Driven.Repositories;
using Medallion.Threading.Postgres;
using Newtonsoft.Json;
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
            var pendingMessages = await _outboxRepository.GetPendingMessagesToProcessAsync(CancellationToken.None);

            Console.WriteLine(DateTime.UtcNow.ToLongTimeString());
            Console.WriteLine(JsonConvert.SerializeObject(pendingMessages, Formatting.Indented));

            await _outboxRepository.MarkEventsAsProcessedAsync(pendingMessages.Select(x => x.Id).ToList());
            
            await Task.Delay(500);
        }
    }
}