using Engine.Application.Enums;

namespace Engine.Application.Common.Errors;

public record Error(
    string? Message,
    string? Code,
    ErrorType ErrorType = ErrorType.Unknown)
{
    public static Error NotFound(string? message = null) =>
        new(message ?? "Resource not found.", "NotFound", ErrorType.NotFound);

    public static Error Validation(string? message = null) =>
        new(message ?? "Invalid input provided.", "InvalidInput", ErrorType.ValidationError);

    public static Error Unauthorized(string? message = null) =>
        new(message ?? "Unauthorized access.", "Unauthorized", ErrorType.Unauthorized);

    public static Error InternalServerError(string? message = null) =>
        new(message ?? 
            "An internal server error occurred.", "InternalServerError", ErrorType.InternalServerError);

    public static Error Forbidden(string? message = null) =>
        new(message ?? "Access to this resource is forbidden.", "Forbidden", ErrorType.Forbidden);

    public static Error Conflict(string? message = null) =>
        new(message ?? "Conflict with the current state of the resource.", "Conflict", ErrorType.Conflict);

    public static Error BadRequest(string? message = null) =>
        new(message ?? "Bad request. Please check your input.", "BadRequest", ErrorType.BadRequest);

    public static Error ServiceUnavailable(string? message = null) =>
        new(message ?? 
            "Service is currently unavailable. Please try again later.", "ServiceUnavailable", 
            ErrorType.ServiceUnavailable);
}


