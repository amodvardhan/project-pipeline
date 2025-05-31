namespace ProjectPipeline.Core.Exceptions;

/// <summary>
/// Exception thrown when business rules are violated
/// </summary>
public class BusinessException : Exception
{
    public string ErrorCode { get; }

    public BusinessException() : base()
    {
        ErrorCode = "BUSINESS_ERROR";
    }

    public BusinessException(string message) : base(message)
    {
        ErrorCode = "BUSINESS_ERROR";
    }

    public BusinessException(string message, string errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }

    public BusinessException(string message, Exception innerException) : base(message, innerException)
    {
        ErrorCode = "BUSINESS_ERROR";
    }

    public BusinessException(string message, string errorCode, Exception innerException) : base(message, innerException)
    {
        ErrorCode = errorCode;
    }
}
