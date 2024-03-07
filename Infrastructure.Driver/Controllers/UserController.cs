using Application.Users.Commands.CreateUser;
using Application.Users.Queries.GetUserById;
using Domain.Entities.Users;
using Domain.Entities.Users.ValueObjects;
using Infrastructure.Driver.Controllers.RequestResource;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Driver.Controllers;

public class UserController : ApiController
{
    public UserController(ISender sender) : base(sender)
    {
    }
    
    [HttpGet("{userId:guid}")]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUser(Guid userId, CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(new UserId(userId));

        var user = await Sender.Send(query, cancellationToken);

        return Ok(user);
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddUser(CreateUserResource createUserResource, CancellationToken cancellationToken)
    {
        var command = new CreateUserCommand(new Name(createUserResource.Name), new Email(createUserResource.Email));

        var user = await Sender.Send(command, cancellationToken);

        return Ok(user);
    }
}