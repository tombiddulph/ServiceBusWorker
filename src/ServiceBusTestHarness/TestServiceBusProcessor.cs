using Azure.Messaging.ServiceBus;

namespace ServiceBusTestHarness;

public class TestServiceBusProcessor : ServiceBusProcessor
{
    internal List<ServiceBusReceivedMessage> Messages { get; } = [];
    internal List<Exception> ErrorMessages { get; } = [];

    public Task ProcessMessage(ProcessMessageEventArgs args) => OnProcessMessageAsync(args);

    protected override Task OnProcessMessageAsync(ProcessMessageEventArgs args)
    {
        Messages.Add(args.Message);

        return base.OnProcessMessageAsync(args);
    }

    protected override Task OnProcessErrorAsync(ProcessErrorEventArgs args)
    {
        ErrorMessages.Add(args.Exception);
        return Task.CompletedTask;
    }
}