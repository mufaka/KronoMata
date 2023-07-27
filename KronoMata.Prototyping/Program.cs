using KronoMata.Data;
using KronoMata.Model;
using KronoMata.Prototyping;

namespace KronoMata.ProtoTyping
{
    internal class Program
    {
#pragma warning disable IDE0060 // Remove unused parameter
        static void Main(string[] args)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            try
            {
                var pluginArchiveRoot = $"{Path.DirectorySeparatorChar}PackageRoot{Path.DirectorySeparatorChar}";
                var machineName = Environment.MachineName;

                // determine if we have initialized data.
                var dataStoreProvider = MockDatabase.Instance.DataStoreProvider;

                // the service method that will run in a loop.
                CheckForJobs(machineName, dataStoreProvider);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static void CheckForJobs(string machineName, IDataStoreProvider dataStoreProvider)
        {
            // is there a host for this system?
            var host = dataStoreProvider.HostDataStore.GetByMachineName(machineName);

            if (host == null)
            {
                // should self register the host? make configurable.
                Console.WriteLine($"There is no Host defined for {machineName}");
            }
            else
            {
                // are there any scheduled jobs defined for this system?
                var scheduledJobs = dataStoreProvider.ScheduledJobDataStore.GetByHost(host.Id);

                if (scheduledJobs.Count == 0)
                {
                    Console.WriteLine($"There are no scheduled jobs defined for {machineName}");
                }
                else
                {
                    foreach (ScheduledJob scheduledJob in scheduledJobs)
                    {
                        Console.WriteLine($"Found Scheduled Job {scheduledJob.Name}");

                        if (ShouldRun(scheduledJob))
                        {
                            // TODO: Obtain plugin zip if not in PackageRoot.

                            // TODO: Extract plugin if not in PackageRoot/<plugin name>_<version>/.

                            // TODO: Dynamically load plugin.

                            // TODO: Execute plugin.

                            // TODO: Log results.
                        }
                    }
                }
            }
        }

        private static bool ShouldRun(ScheduledJob scheduledJob)
        {
            var now = DateTime.Now;

            if (!scheduledJob.IsEnabled) return false;
            if (scheduledJob.EndTime.HasValue && now > scheduledJob.EndTime) return false;

            // TODO: compare current time to job start time and see if any of the interval * step results match

            return true;
        }
    }
}