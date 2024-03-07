using Domain.Abstractions;
using Domain.Entities.Users;
using Domain.Entities.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Driven.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _dbContext;

    public UserRepository(ApplicationDbContext applicationDbContext)
    {
        _dbContext = applicationDbContext;
    }

    public Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Set<User>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<User> AddAsync(User user)
    {
        var response = await _dbContext.Set<User>().AddAsync(user);
        return response.Entity;
    }
}