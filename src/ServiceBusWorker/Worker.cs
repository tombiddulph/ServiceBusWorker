using Azure.Messaging.ServiceBus;

namespace ServiceBusWorker;

public class Worker(ILogger<Worker> logger, ServiceBusClient serviceBusClient)
    : IHostedService
{
    private readonly ServiceBusProcessor _processor = serviceBusClient.CreateProcessor("test-queue");
    private readonly ServiceBusSender _sender = serviceBusClient.CreateSender("test-output-queue");
    private bool _registered;


    private Task OnProcessMessageAsync(ProcessMessageEventArgs arg)
    {
        try
        {
            var message = arg.Message.Body.ToObjectFromJson<TestEvent>();
            _sender.SendMessageAsync(new(arg.Message));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return Task.CompletedTask;
    }

    private Task OnProcessErrorAsync(ProcessErrorEventArgs arg)
    {
        return Task.CompletedTask;
    }


    public Task StartAsync(CancellationToken cancellationToken)
    {
        if (!_registered)
        {
            /*
             * Current the handlers are registered twice, once from Program.cs and once The HostApplicationFactory
             * Ideally the handlers should be registered once
             */
            _processor.ProcessMessageAsync += OnProcessMessageAsync;
            _processor.ProcessErrorAsync += OnProcessErrorAsync;
            _registered = true;
        }

        logger.LogInformation("Worker starting at: {time}", DateTimeOffset.Now);
        return Task.CompletedTask;
    }


    public Task StopAsync(CancellationToken cancellationToken)
    {
        _processor.ProcessMessageAsync -= OnProcessMessageAsync;
        _processor.ProcessErrorAsync -= OnProcessErrorAsync;
        logger.LogInformation("Worker stopping at: {time}", DateTimeOffset.Now);
        return Task.CompletedTask;
    }

    public record TestEvent(string Message);
}