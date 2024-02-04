Poc project demonstrating the usage of Azure.Messaging.ServiceBus alongside Worker Services and in memory message testing.


## ServiceBusWorker
This project is based on the Worker Service template and contains the `Worker` class that is responsible for sending and receiving messages from the Azure Service Bus. The `Worker` class is hosted in the `Program` class and is responsible for creating the `ServiceBusClient` and `ServiceBusSender` and `ServiceBusReceiver` instances.

## ServiceBusTestHarness
This project provides mininmal in memory implmentations of 
- `ServiceBusClient`
- `ServiceBusSender`
- `ServiceBusProcessor`




## Host Application Factory
The `HostApplicationFactory` is a class that is used to create an instance of the `Worker` class and host it in memory. This is useful for testing the worker service without having to run it in a separate process. It's based on this [Stackoverflow post](https://stackoverflow.com/questions/77371811/is-there-an-equivalent-for-webapplicationfactory-for-a-net-core-console-app/77734096#77734096).


## ServiceBusWorker.Tests
This project contains the tests for the `ServiceBusWorker` project. It uses the `HostApplicationFactory` to create an instance of the `Worker` class and host it in memory. The tests then use the `ServiceBusClient` to send and receive messages from the Azure Service Bus.

