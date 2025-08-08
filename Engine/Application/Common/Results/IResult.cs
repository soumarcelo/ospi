using Engine.Application.Common.Errors;

namespace Engine.Application.Common.Results;

public interface IResult
{
    public bool IsSuccess { get; }
    public Error? Error { get; }
}

public interface IResult<T> : IResult
{
    public T? Value { get; }
}
