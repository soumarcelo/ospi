namespace Engine.Application.Common.Errors;

public record Error(
    string? Message,
    string? Code);
