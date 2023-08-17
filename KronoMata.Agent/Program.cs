using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace KronoMata.Agent
{
    internal class Program
    {
#pragma warning disable IDE0060 // Remove unused parameter
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        static async Task Main(string[] args)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
#pragma warning restore IDE0060 // Remove unused parameter
        {
            try
            {
                IConfiguration configuration = new ConfigurationBuilder()
                      .SetBasePath(Directory.GetCurrentDirectory())
                      .AddJsonFile("appsettings.json", false)
                      .Build();

                var agentHost = new HostBuilder()
                    .ConfigureHostConfiguration(host => { })
                    .ConfigureServices((hostContext, services) =>
                    {
                        services.AddLogging(builder =>
                        {
                            builder.ClearProviders();
                            builder.AddConfiguration(configuration.GetSection("Logging"));
                            builder.AddConsole();
                            builder.AddDebug();
                        });
                        services.AddSingleton(configuration);
                        services.AddSingleton<IHostedService, PluginRunner>();
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