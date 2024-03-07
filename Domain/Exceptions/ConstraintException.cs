namespace Domain.Exceptions;

// generate also a custom exception for constraint violations

public sealed class ConstraintException : ArgumentException
{
    public ConstraintException(string message) : base(message)
    {
    }
    public ConstraintException(string message, string paramName) : base(message, paramName)
    {
    }
}
