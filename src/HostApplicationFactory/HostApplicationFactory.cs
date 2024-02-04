using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HostApplicationFactory;

public sealed class HostApplicationFactory<TEntryPoint>(Action<IWebHostBuilder> configuration)
    : WebApplicationFactory<TEntryPoint>
    where TEntryPoint : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder) =>
        configuration(builder.Configure(_ => { }));

    public Task RunHostAsync(CancellationToken cancellationToken = default) =>
        Services.GetRequiredService<IHost>().WaitForShutdownAsync(cancellationToken);

    public void StartHost() => Services.GetRequiredService<IHost>().Start();

    public async Task StartHostAsync(CancellationToken cancellationToken = default) =>
        await Services.GetRequiredService<IHost>().StartAsync(cancellationToken);

    public void StopHost() => Services.GetRequiredService<IHost>().StopAsync().GetAwaiter().GetResult();

    public async Task StopHostAsync(CancellationToken cancellationToken = default) =>
        await Services.GetRequiredService<IHost>().StopAsync(cancellationToken);
}