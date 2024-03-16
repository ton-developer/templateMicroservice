using Application.Abstractions;
using Domain.Entities.Users;
using Domain.Entities.Users.ValueObjects;

namespace Application.Users.Commands.CreateUser;

public record CreateUserCommand(Name Name, Email Email) : ICommand<User>;