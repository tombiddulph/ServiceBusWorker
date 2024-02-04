using Azure.Messaging.ServiceBus;

namespace ServiceBusTestHarness;

public class TestServiceBusSender(ServiceBusClient client, string queueName) : ServiceBusSender
{
    private readonly TestServiceBusClient _client = (client as TestServiceBusClient)!;
    private readonly List<ServiceBusMessage> _messages = [];
    internal List<ServiceBusMessage> Messages => _messages;
    internal List<Exception> ErrorMessages { get; } = [];

    private readonly string _queueName = queueName;

    public override Task SendMessagesAsync(ServiceBusMessageBatch messageBatch,
        CancellationToken cancellationToken = default)
    {
        //todo: implement
        throw new NotImplementedException("Not implemented");
    }

    public override async Task SendMessagesAsync(IEnumerable<ServiceBusMessage> messages,
        CancellationToken cancellationToken = default)
    {
        foreach (var message in messages)
        {
            await SendMessageAsync(message, cancellationToken);
        }
    }

    public override async Task SendMessageAsync(ServiceBusMessage message,
        CancellationToken cancellationToken = default)
    {
        _messages.Add(message);
        await _client.SendMessage(_queueName, message, cancellationToken);
    }
}