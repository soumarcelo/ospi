using Engine.Application.Common.Errors;
using Engine.Application.Enums;

namespace Engine.Application.Common.Results;

public class FailureResult(
    string errorMessage, string? errorCode = null, ErrorType errorType = ErrorType.Unknown) : IResult
{
    public bool IsSuccess => false;
    public Error? Error { get; } = new(errorMessage, errorCode, errorType);
}

public class FailureResult<TValue>(
    string errorMessage, string? errorCode = null, ErrorType errorType = ErrorType.Unknown) : IResult<TValue>
{
    public bool IsSuccess => false;
    public Error? Error { get; } = new(errorMessage, errorCode, errorType);
    public TValue? Value => default;
}