using Application.Abstractions;
using Application.Abstractions.Data;
using Domain.Abstractions;
using Domain.Entities.Users;

namespace Application.Users.Commands.CreateUser;

public sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, User>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    
    public CreateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<User> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var user = User.Create(command.Name, command.Email);
        user = await _userRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return user;
    }
}