using System.Collections.Immutable;
using Azure.Messaging.ServiceBus;

namespace ServiceBusTestHarness;

public class TestServiceBusClient : ServiceBusClient
{
    private readonly Dictionary<string, TestServiceBusProcessor> _processors = [];
    private readonly Dictionary<string, TestServiceBusSender> _senders = [];

    public ImmutableDictionary<string, ImmutableList<ServiceBusReceivedMessage>> ReceivedMessages =>
        _processors.ToImmutableDictionary(p => p.Key, p => p.Value.Messages.ToImmutableList());

    public ImmutableDictionary<string, ImmutableList<ServiceBusMessage>> SentMessages =>
        _senders.ToImmutableDictionary(s => s.Key, s => s.Value.Messages.ToImmutableList());

    public IReadOnlyCollection<Exception> ErrorMessages =>
        _processors.SelectMany(p => p.Value.ErrorMessages).ToImmutableList();

    public IReadOnlyCollection<string> ProcessorQueues => _processors.Keys;

    internal TestServiceBusClient()
    {
    }

    public Task SendMessage<T>(string queueName, T message, CancellationToken token = default) =>
        SendMessage(queueName,
            ServiceBusModelFactory.ServiceBusReceivedMessage(BinaryData.FromObjectAsJson(message)), token);


    public Task SendMessage(string queueName, object message, CancellationToken token = default) =>
        SendMessage(queueName,
            ServiceBusModelFactory.ServiceBusReceivedMessage(BinaryData.FromObjectAsJson(message)), token);

    public async Task SendMessage(string queueName, ServiceBusReceivedMessage message,
        CancellationToken token = default)
    {
        if (_processors.TryGetValue(queueName, out var processor))
        {
            await processor.ProcessMessage(new(message, null, token));
        }
        else
        {
            throw new InvalidOperationException($"Queue: {queueName} not found");
        }
    }

    public override ServiceBusProcessor CreateProcessor(string queueName)
    {
        if (_processors.TryGetValue(queueName, out var p))
        {
            return p;
        }

        TestServiceBusProcessor processor = new();
        _processors.Add(queueName, processor);
        return processor;
    }

    public override ServiceBusSender CreateSender(string queueName)
    {
        if (_senders.TryGetValue(queueName, out var s))
        {
            return s;
        }

        var sender = new TestServiceBusSender(this, queueName);
        _senders.Add(queueName, sender);
        return sender;
    }

    public override ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        return new(Task.CompletedTask);
    }
}