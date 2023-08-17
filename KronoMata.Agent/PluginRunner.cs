using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace KronoMata.Agent
{
    internal class PluginRunner : IHostedService
    {
        private readonly ILogger<PluginRunner> _logger;
        private readonly IConfiguration _configuration;
        private readonly PeriodicTimer _periodicTimer = new(TimeSpan.FromMinutes(1));

        public PluginRunner(ILogger<PluginRunner> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("KronoMata Agent started.");

            await ExecuteAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("KronoMata Agent stopped.");
            _periodicTimer.Dispose();
            return Task.CompletedTask;
        }

        private async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                while (await _periodicTimer.WaitForNextTickAsync(cancellationToken)
                && !cancellationToken.IsCancellationRequested)
                {
                    //CheckForJobs();
                    _logger.LogInformation("{time} Agent checking for jobs.", DateTime.Now.ToString("O"));
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected errror executing agent. {ex.Message", ex.Message);
            }
        }
    }
}
