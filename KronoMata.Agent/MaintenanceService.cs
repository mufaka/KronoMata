using KronoMata.Model;
using KronoMata.Scheduling;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace KronoMata.Agent
{
    internal class MaintenanceService : BackgroundService
    {
        private readonly ILogger<MaintenanceService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IShouldRun _shouldRun;
        private readonly PeriodicTimer _periodicTimer;
        private readonly ScheduledJob _maintenanceJob;

        public MaintenanceService(ILogger<MaintenanceService> logger, 
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory,
            IShouldRun shouldRun)
        {
            _logger = logger;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _shouldRun = shouldRun;
            _maintenanceJob = CreateMockMaintenanceJob();
            _periodicTimer = new(TimeSpan.FromMinutes(1));
        }

        private ScheduledJob CreateMockMaintenanceJob()
        {
            var maintenanceIntervalConfiguration = _configuration["KronoMata:MaintenanceInterval"] ?? "15";
            int maintenanceInterval;

            if (!Int32.TryParse(maintenanceIntervalConfiguration, out maintenanceInterval))
            {
                maintenanceInterval = 15;
            }

            var scheduledJob = new ScheduledJob();

            // kick off 1 minute after starting service (set start time accordingly)
            scheduledJob.StartTime = DateTime.Now.AddMinutes(-(maintenanceInterval - 1));
            scheduledJob.IsEnabled = true;
            scheduledJob.Frequency = ScheduleFrequency.Minute;
            scheduledJob.Interval = maintenanceInterval;

            return scheduledJob;
        }

        private DateTime? _lastTick;

        protected async override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("KronoMata Agent Maintenance Service Started.");

                while (await _periodicTimer.WaitForNextTickAsync(cancellationToken)
                && !cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        if (_lastTick == null)
                        {
                            _lastTick = DateTime.Now;
                        }
                        else
                        {
                            var now = DateTime.Now;

                            if (now.Minute == _lastTick.Value.Minute)
                            {
                                continue;
                            }

                            _lastTick = now;
                        }

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                        if (_shouldRun.ShouldRun(DateTime.Now, _maintenanceJob))
                        {
                            Task.Run(() => CheckForPackages());
                        }
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Unexpected error checking for packages. {ex.Message}", ex.Message);
                    }
                }

                _logger.LogInformation("KronoMata Agent Maintenance Job stopped.");
                _periodicTimer.Dispose();
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected errror executing maintenance. {ex.Message}", ex.Message);
            }
        }

        private void CheckForPackages()
        {
            var apiClient = new ApiClient(_configuration, _httpClientFactory);

            _logger.LogInformation("Checking API for installed packages.");
            var packages = apiClient.GetAllPackages();
            var packageRoot = _configuration["KronoMata:PackageRoot"];


            // The only files or directories that exist should match existing
            // package names.
            if (Directory.Exists(packageRoot))
            {
                var files = Directory.GetFiles(packageRoot);

                foreach (var file in files)
                {
                    var packageFileName = Path.GetFileName(file);
                    var package = packages.Where(p => p.FileName == packageFileName).FirstOrDefault();

                    if (package == null)
                    {
                        File.Delete(file);
                    }
                }

                var directories = Directory.GetDirectories(packageRoot);

                foreach (var directory in directories)
                {
                    var packageFileName = $"{directory}.zip";
                    var package = packages.Where(p => p.FileName == packageFileName).FirstOrDefault();

                    if (package == null)
                    {
                        Directory.Delete(directory, true);
                    }
                }
            }
        }
    }
}
