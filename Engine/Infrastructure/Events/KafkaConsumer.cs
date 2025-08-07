using Confluent.Kafka;
using Engine.Application.Interfaces;

namespace Engine.Infrastructure.Events;

public class KafkaConsumer<TKey, TValue, TEventHandler>(
    IConsumer<TKey, TValue> consumer,
    TEventHandler eventHandler,
    ILogger<KafkaConsumer<TKey, TValue, TEventHandler>> logger) : BackgroundService
        where TEventHandler : class, IEventHandler<TKey, TValue>
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken) =>
        Task.Run(() => StartConsumerLoop(stoppingToken), stoppingToken);

    private async Task StartConsumerLoop(CancellationToken stoppingToken)
    {
        string topic = consumer.Subscription.First();
        consumer.Subscribe(topic);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                ConsumeResult<TKey, TValue> result = consumer.Consume(stoppingToken);
                if (result is null) continue;

                await eventHandler.ProcessMessageAsync(
                    result.Message.Key,
                    result.Message.Value,
                    stoppingToken);
            }
            catch (ConsumeException ex)
            {
                logger.LogError("Error consuming message: {reason}", ex.Error.Reason);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ocorreu um erro inesperado no consumidor.");
            }
        }
    }

    public override void Dispose()
    {
        consumer.Close();
        consumer.Dispose();
        base.Dispose();

        GC.SuppressFinalize(this);
    }
}
