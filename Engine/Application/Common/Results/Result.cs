using Engine.Application.Common.Errors;
using Engine.Application.Enums;

namespace Engine.Application.Common.Results;

public static class Result
{
    public static IResult Success() => new SuccessResult();
    public static IResult<TValue> Success<TValue>(TValue value) => new SuccessResult<TValue>(value);

    public static IResult Failure(
        string errorMessage,
        string? errorCode = null,
        ErrorType errorType = ErrorType.Unknown)
        => new FailureResult(errorMessage, errorCode, errorType);

    public static IResult<TValue> Failure<TValue>(
        string errorMessage,
        string? errorCode = null,
        ErrorType errorType = ErrorType.Unknown)
        => new FailureResult<TValue>(errorMessage, errorCode, errorType);

    public static bool TryGetValue<T>(
        this IResult<T> result,
        out T value,
        out Error? error)
    {
        if (result.IsSuccess && result.Value is not null)
        {
            value = result.Value;
            error = null;
            return true;
        }

        value = default!;
        error = result.Error;
        return false;
    }

}