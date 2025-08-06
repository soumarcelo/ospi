namespace Engine.Application.Common.Results;

public class FailureResult(string errorMessage, string? errorCode = null) : IResult
{
    public bool IsSuccess => false;
    public string? ErrorMessage { get; } = errorMessage;
    public string? ErrorCode { get; } = errorCode;
}

public class FailureResult<TValue>(string errorMessage, string? errorCode = null) : IResult<TValue>
{
    public bool IsSuccess => false;
    public string? ErrorMessage { get; } = errorMessage;
    public string? ErrorCode { get; } = errorCode;
    public TValue? Value => default;
}