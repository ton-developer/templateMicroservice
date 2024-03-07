using Domain.Entities;
using Domain.Entities.Users;
using Domain.Entities.Users.ValueObjects;

namespace Domain.Abstractions;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken = default);
    Task<User> AddAsync(User user);
}