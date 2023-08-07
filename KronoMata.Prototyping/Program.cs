using KronoMata.Data.Mock;
using KronoMata.Prototyping;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace KronoMata.ProtoTyping
{
    internal class Program
    {
#pragma warning disable IDE0060 // Remove unused parameter
        static async Task Main(string[] args)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            try
            {
                var agentHost = new HostBuilder()
                    .ConfigureHostConfiguration(host => { })
                    .ConfigureServices((hostContext, services) =>
                    {
                        services.AddSingleton(MockDatabase.Instance.DataStoreProvider);
                        services.AddSingleton<IHostedService, AgentPrototype>();
                    })
                    .UseConsoleLifetime()
                    .Build();

                // suppressing this warning because we wan't the call to be async
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                agentHost.RunAsync();

                Console.ReadLine();

                agentHost.StopAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}