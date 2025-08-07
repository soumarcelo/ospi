
using Confluent.Kafka;
using Engine.Application.Interfaces;
using Engine.Infrastructure.Events;
using Microsoft.Extensions.Options;

namespace Engine.Presentation.Extensions;

public static class KafkaServiceCollectionExtensions
{
    public static IServiceCollection AddKafkaConsumerService<TKey, TValue, TEventHandler>(
        this IServiceCollection services,
        string topic)
        where TEventHandler : class, IEventHandler<TKey, TValue>
    {
        services.AddSingleton(sp =>
        {
            KafkaConfig kafkaConfig = sp.GetRequiredService<IOptions<KafkaConfig>>().Value;
            ConsumerConfig consumerConfig = new()
            {
                BootstrapServers = kafkaConfig.BootstrapServers,
                GroupId = kafkaConfig.ConsumerGroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
            };

            IConsumer<string, string> consumer = new ConsumerBuilder<string, string>(consumerConfig)
                .SetKeyDeserializer(Deserializers.Utf8)
                .SetValueDeserializer(Deserializers.Utf8)
                .Build();

            consumer.Subscribe(topic);

            return consumer;
        });

        services.AddScoped<TEventHandler>();

        services.AddHostedService<KafkaConsumer<TKey, TValue, TEventHandler>>();

        return services;
    }
}