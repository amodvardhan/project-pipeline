namespace ProjectPipeline.Core.Exceptions;

/// <summary>
/// Exception thrown when user is not authorized to perform an action
/// </summary>
public class UnauthorizedException : Exception
{
    public UnauthorizedException() : base("Unauthorized access.")
    {
    }

    public UnauthorizedException(string message) : base(message)
    {
    }

    public UnauthorizedException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
