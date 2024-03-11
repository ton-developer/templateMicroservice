using Domain.Entities.Primitives;
using Domain.Entities.Users.Events;
using Domain.Entities.Users.ValueObjects;
using Domain.Primitives;

namespace Domain.Entities.Users;

public class User : AggregateRoot<UserId>
{
    public Name Name { get; private set; }
    public Email Email { get; private set; }
    
    private User(UserId id, Name name, Email email) : base(id)
    {
        Name = name;
        Email = email;
    }
    
    public static User Create(Name name, Email email)
    {
        var user = new User(new UserId(Guid.NewGuid()), name, email);
        user.Raise(new UserCreatedDomainEvent(user.Id, name, email));
        return user;
    }
}

