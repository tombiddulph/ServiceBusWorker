using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ServiceBusTestHarness;

public static class ServiceCollectionExtensions
{
    private static readonly Type ServiceBusClientType = typeof(ServiceBusClient);

    public static TestServiceBusClient AddTestServiceBusClient(this IServiceCollection services)
    {
        var client = new TestServiceBusClient();
        services.RemoveAll(ServiceBusClientType).AddSingleton<ServiceBusClient, TestServiceBusClient>(_ => client);

        return client;
    }
}