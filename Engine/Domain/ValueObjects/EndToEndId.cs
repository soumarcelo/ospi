namespace Engine.Domain.ValueObjects;

public record EndToEndId
{
    public string Value { get; init; }

    public EndToEndId()
    {
        // 1. Gerar o Timestamp (AAAAMMDDHH - UTC)
        // DateTime.UtcNow garante que a hora seja em UTC, conforme a especificação.
        string timestamp = DateTime.UtcNow.ToString("yyyyMMddHH"); // 10 caracteres

        // 2. Gerar o Identificador Único (22 caracteres alfanuméricos)
        // Você pode usar Guid para alta probabilidade de unicidade e depois formatar.
        // Convertendo Guid para string e removendo hifens, depois pegando uma parte.
        // Para garantir 22 caracteres alfanuméricos e evitar caracteres especiais do Guid,
        // é melhor usar um gerador de strings aleatórias.

        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        char[] randomChars = new char[22];
        Random random = new();

        for (int i = 0; i < randomChars.Length; i++)
        {
            randomChars[i] = chars[random.Next(chars.Length)];
        }
        string uniqueId = new(randomChars); // 22 caracteres

        // Concatena as partes
        Value = $"E{timestamp}{uniqueId}";
    }

    public EndToEndId(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !value.StartsWith('E') || value.Length != 32)
        {
            throw new ArgumentException("Invalid EndToEndId format.", nameof(value));
        }
        Value = value;
    }

    public override string ToString() => Value;
}
