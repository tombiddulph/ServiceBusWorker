using Microsoft.Extensions.Azure;
using ServiceBusWorker;


const string connectionString = "some connection string";

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddAzureClients(clientsBuilder => clientsBuilder.AddServiceBusClient(connectionString));


var host = builder.Build();
host.Run();

public partial class Program;