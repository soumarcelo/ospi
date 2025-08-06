namespace Engine.Domain.ValueObjects;

public record TransactionCounterparty(
    string InstitutionName,
    string InstitutionISPB,
    string LegalName,
    string Document);
