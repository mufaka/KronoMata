using KronoMata.Model;
using KronoMata.Prototyping;

namespace KronoMata.ProtoTyping
{
    internal class Program
    {
        static void Main(string[] args) 
        {
            try
            {
                var pluginArchiveRoot = $"{Path.DirectorySeparatorChar}PackageRoot{Path.DirectorySeparatorChar}";
                var machineName = Environment.MachineName;

                // determine if we have initialized data.
                var dataStoreProvider = MockDatabase.Instance.DataStoreProvider;

                // is there a host for this system?
                var host = dataStoreProvider.HostDataStore.GetByMachineName(machineName);

                if (host == null)
                {
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
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}