namespace Engine.Application.Common.Results;

public class SuccessResult : IResult
{
    public bool IsSuccess => true;
    public string? ErrorMessage => null;
    public string? ErrorCode => null;
}

public class SuccessResult<TValue>(TValue? value) : IResult<TValue>
{
    public bool IsSuccess => true;
    public string? ErrorMessage => null;
    public string? ErrorCode => null;

    public TValue? Value { get; } = value;
}