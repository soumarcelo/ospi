using Confluent.Kafka;

namespace Engine.Infrastructure.Events;

public class KafkaProducer(ProducerConfig config, Logger<KafkaProducer> logger) : IDisposable
{
    private readonly IProducer<Null, string> _producer = 
        new ProducerBuilder<Null, string>(config).Build();

    public async Task ProduceAsync(string topic, string message)
    {
        try
        {
            DeliveryResult<Null, string> result = 
                await _producer.ProduceAsync(topic, new Message<Null, string> { Value = message });

            logger.LogInformation(
                "Message produced to topic {Topic} with offset {Offset} and partition {Partition}",
                topic, result.Offset, result.Partition);
        }
        catch (ProduceException<Null, string> ex)
        {
            logger.LogError(
                ex,
                "Failed to produce message to topic {Topic}: {ErrorMessage}",
                topic, ex.Error.Reason);
            throw new Exception($"Error producing message to topic {topic}: {ex.Message}", ex);
        }
    }

    public void Dispose()
    {
        _producer?.Dispose();
        GC.SuppressFinalize(this);
    }
}
