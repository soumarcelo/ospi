using Engine.Application.Common.Errors;

namespace Engine.Application.Common.Results;

public class SuccessResult : IResult
{
    public bool IsSuccess => true;
    public Error? Error => null;
}

public class SuccessResult<TValue>(TValue? value) : IResult<TValue>
{
    public bool IsSuccess => true;
    public Error? Error => null;

    public TValue? Value { get; } = value;
}