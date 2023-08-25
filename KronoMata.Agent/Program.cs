using KronoMata.Scheduling;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace KronoMata.Agent
{
    internal class Program
    {
#pragma warning disable IDE0060 // Remove unused parameter
        static async Task Main(string[] args)
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
                        services.AddScoped<IShouldRun, RecurrenceShouldRun>();
                        services.AddHttpClient();

                        services.AddSingleton<IHostedService, PluginRunner>();
                    })
                    .UseConsoleLifetime()
                    .Build();

                await agentHost.RunAsync();
                Console.ReadLine();
                await agentHost.StopAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}