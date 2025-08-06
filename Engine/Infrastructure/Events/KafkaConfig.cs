namespace Engine.Infrastructure.Events;

public class KafkaConfig
{
    public string BootstrapServers { get; set; } = string.Empty;
    public string ConsumerGroupId { get; set; } = string.Empty;
}