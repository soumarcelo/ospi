namespace Engine.Domain.ValueObjects;

public record AuthorizationPermission
{
    public enum TargetAction
    {
        Create,
        Read,
        Update,
        Delete,
        List,
        Export,
        Import

    }

    public string Resource { get; init; }
    public TargetAction Action { get; init; }

    public AuthorizationPermission(string resource, TargetAction action)
    {
        ValidateResource(resource);

        Resource = resource;
        Action = action;
    }

    public AuthorizationPermission(string resource, string action)
        : this(resource, ParseAction(action))
    {
    }

    public AuthorizationPermission(string permission)
    {
        (string resource, TargetAction action) = ParsePermissionString(permission);
        Resource = resource;
        Action = action;
    }

    private static (string resource, TargetAction action) ParsePermissionString(string permission)
    {
        string[] parts = permission.Split('.');
        if (parts.Length != 2)
            throw new ArgumentException("Permission must be in the format 'Resource.Action'", nameof(permission));

        ValidateResource(parts[0]);
        TargetAction action = ParseAction(parts[1]);

        return (parts[0], action);
    }

    private static void ValidateResource(string resource)
    {
        if (string.IsNullOrWhiteSpace(resource))
            throw new ArgumentException("Resource cannot be empty.", nameof(resource));
    }

    private static TargetAction ParseAction(string action)
    {
        if (!Enum.TryParse(action, true, out TargetAction parsedAction))
            throw new ArgumentException($"Invalid action '{action}' in permission.", nameof(action));

        return parsedAction;
    }

    public override string ToString()
    {
        return $"{Resource}.{Action}";
    }
}
