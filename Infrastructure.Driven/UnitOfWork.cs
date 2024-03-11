using Application.Abstractions.Data;

namespace Infrastructure.Driven;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;

    public UnitOfWork(ApplicationDbContext dbContext) => _dbContext = dbContext;

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _dbContext.SaveChangesAsync(cancellationToken);
}