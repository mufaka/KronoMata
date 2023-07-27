using KronoMata.Data;
using KronoMata.Model;
using KronoMata.Prototyping;
using System.IO.Compression;
using System.Text.RegularExpressions;

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
                    var pluginArchiveRoot = $"PackageRoot{Path.DirectorySeparatorChar}";

                    foreach (ScheduledJob scheduledJob in scheduledJobs)
                    {
                        Console.WriteLine($"Found Scheduled Job {scheduledJob.Name}");

                        if (ShouldRun(scheduledJob))
                        {
                            var pluginMetaData = dataStoreProvider.PluginMetaDataDataStore.GetById(scheduledJob.PluginMetaDataId);
                            var package = dataStoreProvider.PackageDataStore.GetById(pluginMetaData.PackageId);
                            var packageFolder = $"{pluginArchiveRoot}{GetPluginFolderName(pluginMetaData)}";
                            var packageArchivePath = $"{pluginArchiveRoot}{package.FileName}";

                            if (!Directory.Exists(packageFolder))
                            {
                                if (!File.Exists(packageArchivePath))
                                {
                                    // TODO: attempt to fetch from future API.
                                    Console.WriteLine($"Could not find package path at {packageArchivePath}");
                                }
                                else
                                {
                                    // need to extract archive to packageFolder
                                    Console.WriteLine($"Found package archive at {packageArchivePath}. Unzipping to {packageFolder}");
                                    ZipFile.ExtractToDirectory(packageArchivePath, packageFolder);
                                }
                            }

                            // the work above should result in this folder now being available
                            if (!Directory.Exists(packageFolder))
                            {
                                Console.WriteLine($"Unable to find package folder at {packageFolder}");
                            }
                            else
                            {
                                // now what? PluginLoadContext? Do we require the assembly file to be named
                                // a certain way? eg: PluginMetaData.AssemblyName.dll? I think so ...

                                // TODO: Map configuration values.

                                // TODO: Dynamically load plugin.

                                // TODO: Execute plugin.

                                // TODO: Log results.
                            }
                        }
                    }
                }
            }
        }

        private static string GetPluginFolderName(PluginMetaData metaData)
        {
            // NOTE: Not too worried about Regex performance here as it
            // NOTE: isn't in a hot spot.

            // only allow [0-9a-zA-Z-.] characters in the filename as they
            // are safe on 'all' platforms (Windows, Mac, *nix)

            // replace invalid characters with spaces
            var folderName = Regex.Replace($"{metaData.Name}_{metaData.Version}", @"[^0-9a-zA-Z-.]", " ");

            // replace multiple spaces with a single space
            folderName = Regex.Replace(folderName, "[ ]{2,}", " ");

            // replace single spaces with _
            folderName = Regex.Replace(folderName, "[ ]", "_");

            return folderName;
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