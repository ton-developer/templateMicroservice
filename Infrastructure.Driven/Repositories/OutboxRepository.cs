using Dapper;
using Infrastructure.Driven.Outbox;
using Npgsql;

namespace Infrastructure.Driven.Repositories;

public class OutboxRepository
{
    private readonly string _connectionString;

    public OutboxRepository(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public async Task<IEnumerable<OutboxMessage>> GetPendingMessagesToProcessAsync(CancellationToken cancellationToken)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        var sql =
            """SELECT * FROM public."outbox_messages" WHERE "ProcessedOnUtc" IS NULL ORDER BY "OccurredOnUtc" LIMIT 100""";
        return await connection.QueryAsync<OutboxMessage>(sql, cancellationToken);
    }
    
    public async Task<int> MarkEventsAsProcessedAsync(List<Guid> ids)
    {
        if (!ids.Any())
        {
            return 0;
        }
        
        var sql = """UPDATE public."outbox_messages" SET "ProcessedOnUtc" = NOW() WHERE "Id" = ANY(@Ids)""";
    
        await using var connection = new NpgsqlConnection(_connectionString);
        return await connection.ExecuteAsync(sql, new { Ids = ids });
    }
}