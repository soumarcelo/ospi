using Engine.Application.Common.Errors;

namespace Engine.Application.Common.Results;

public static class Result
{
    public static IResult Success() => new SuccessResult();
    public static IResult<TValue> Success<TValue>(TValue value) => new SuccessResult<TValue>(value);

    public static IResult Failure(
        string errorMessage,
        string? errorCode = null)
        => new FailureResult(errorMessage, errorCode);

    public static IResult<TValue> Failure<TValue>(
        string errorMessage,
        string? errorCode = null)
        => new FailureResult<TValue>(errorMessage, errorCode);

    public static bool TryGetValue<T>(
        this IResult<T> result,
        out T value,
        out Error? error)
    {
        if (result.IsSuccess && result.Value is not null)
        {
            value = result.Value;
            error = new(null, null);
            return true;
        }

        value = default!;
        error = new(result.ErrorMessage, result.ErrorCode);
        return false;
    }

}