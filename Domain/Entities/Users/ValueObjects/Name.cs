using Domain.Entities.Primitives;
using Domain.Exceptions;

namespace Domain.Entities.Users.ValueObjects;

public record Name : StringValue
{
    public Name(string value) : base(value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ConstraintException("Value cannot be null or whitespace.", nameof(value));
        }
            
        if (value.Length > 100)
        {
            throw new ConstraintException("Value cannot be more than 100 characters.", nameof(value));
        }
            
        Value = value;
    }
}