using System.Collections.Immutable;
using Azure.Messaging.ServiceBus;
using HostApplicationFactory;
using Microsoft.AspNetCore.TestHost;
using ServiceBusTestHarness;

namespace ServiceBusWorker.Tests;

public abstract class ServiceBusWorkerFixture : IAsyncLifetime
{
    private TestServiceBusClient _serviceBusClient = null!;
    private HostApplicationFactory<Program> _factory = null!;

    internal ImmutableList<ServiceBusReceivedMessage> GetReceivedMessagesForQueue(string queueName) =>
        _serviceBusClient.ReceivedMessages.TryGetValue(queueName, out var messages) switch
        {
            true => messages,
            false => ImmutableList<ServiceBusReceivedMessage>.Empty
        };

    internal ImmutableList<ServiceBusMessage> GetSentMessagesForQueue(string queueName) =>
        _serviceBusClient.SentMessages.TryGetValue(queueName, out var messages) switch
        {
            true => messages,
            false => ImmutableList<ServiceBusMessage>.Empty
        };


    public async Task InitializeAsync()
    {
        _factory = new(builder => builder.ConfigureServices(services =>
        {
            _serviceBusClient = services.AddTestServiceBusClient();
        }));
        await _factory.StartHostAsync();
    }


    public Task SendMessage(string queueName, object message, CancellationToken token = default) =>
        _serviceBusClient.SendMessage(queueName, message, token);

    public Task SendMessage<T>(string queueName, T message, CancellationToken token = default) =>
        _serviceBusClient.SendMessage(queueName, message, token);

    public Task SendMessage(string queueName, ServiceBusReceivedMessage message, CancellationToken token = default) =>
        _serviceBusClient.SendMessage(queueName, message, token);


    public async Task DisposeAsync() => await _factory.StopHostAsync();
}