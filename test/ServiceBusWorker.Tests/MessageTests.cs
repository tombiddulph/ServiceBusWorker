using FluentAssertions;

namespace ServiceBusWorker.Tests;

public class MessageTests : ServiceBusWorkerFixture
{
    [Fact]
    public async Task TestQueueReceivesMessage()
    {
        var message = new Worker.TestEvent("Tom");
        await SendMessage("test-queue", message);

        var messages = GetReceivedMessagesForQueue("test-queue");
        messages.Should().ContainSingle();
        var receivedMessage = messages[0].Body.ToObjectFromJson<Worker.TestEvent>();
        receivedMessage.Should().BeEquivalentTo(new
        {
            Message = "Tom",
        });
    }

    [Fact]
    public async Task TestOutputQueueSendsMessage()
    {
        var message = new Worker.TestEvent("Tom");
        await SendMessage("test-queue", message);

        var messages = GetSentMessagesForQueue("test-output-queue");
        messages.Should().ContainSingle();
        var receivedMessage = messages[0].Body.ToObjectFromJson<Worker.TestEvent>();
        receivedMessage.Should().BeEquivalentTo(new
        {
            Message = "Tom",
        });
    }
}