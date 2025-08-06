using Engine.Domain.Enums;
using Engine.Domain.ValueObjects;

namespace Engine.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string FullName { get; private set; }
    public EmailAddress ContactEmail { get; private set; }
    //public Phone ContactPhone { get; private set; }
    public IndividualDocument Document { get; private set; }
    public UserStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public User()
    {
        FullName = string.Empty;
        ContactEmail = EmailAddress.Invalid;
        Document = IndividualDocument.Invalid;
    }

    public static User Create(string fullName, EmailAddress contactEmail, IndividualDocument document)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Full name cannot be null or empty.", nameof(fullName));

        return new User
        {
            Id = Guid.NewGuid(),
            FullName = fullName,
            ContactEmail = contactEmail,
            Document = document,
            Status = UserStatus.Active,
            CreatedAt = DateTime.UtcNow
        };
    }
}
