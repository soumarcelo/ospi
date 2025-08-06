namespace Engine.Domain.Enums;

public enum OutboxMessageStatus
{
    Pending,
    Published,
    Failed
}

public enum OutboxMessageFailureReasonCode
{
    Unknown,
    InvalidData,
    ProcessingError,
    Timeout,
    DuplicateMessage,
    ExceededMaxAttempts,
}