using Domain.Entities.Primitives;
using Domain.Exceptions;

namespace Domain.Entities.Users.ValueObjects;

public record Email : StringValue
{
    public Email(string value) : base(value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ConstraintException("Value cannot be null or whitespace.", nameof(value));
        }
        if (value.Length > 250)
        {
            throw new ConstraintException("Value cannot be more than 100 characters.", nameof(value));
        }
        if (!value.Contains('@'))
        {
            throw new ConstraintException("Value must contain '@'.", nameof(value));
        }
        Value = value;
    }
}