using Application.Abstractions;
using Domain.Abstractions;
using Domain.Entities.Users;

namespace Application.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, User?>
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<User?> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        return _userRepository.GetByIdAsync(query.Id, cancellationToken);
    }
}