using Engine.Application.Interfaces;

namespace Engine.Infrastructure.Events.Handlers;

public class InternalPixTransferCreditRequestedEventHandler(
    Logger<InternalPixTransferCreditRequestedEventHandler> logger) : IEventHandler<string, string>
{
    public static string Topic => "pix.internal.transfer.credit";

    public async Task ProcessMessageAsync(string key, string value, CancellationToken cancellationToken)
    {
        // Process the message here
        logger.LogInformation(
            "Received message: {message} with key: {key}", value, key);

        // Simulate async processing
        await Task.Delay(100, cancellationToken);
    }
}
