using Engine.Application.DTOs.Transactions;

namespace Engine.Application.Common.Results;

public interface IResult
{
    public bool IsSuccess { get; }
    public string? ErrorMessage { get; }
    public string? ErrorCode { get; }
}

public interface IResult<T> : IResult
{
    public T? Value { get; }
}
