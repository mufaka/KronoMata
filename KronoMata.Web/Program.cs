using FluentValidation;
using KronoMata.Data;
using KronoMata.Data.InMemory;
using KronoMata.Data.Mock;
using KronoMata.Data.SQLite;
using KronoMata.Model;
using KronoMata.Model.Validation;
using System.Reflection;

namespace KronoMata.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

#if DEBUG
            var config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.Development.json", optional: false)
                    .Build();
#else
            var config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: false)
                    .Build();
#endif 

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var databaseProvider = config["KronoMata:DataStoreProvider"];

            if (databaseProvider != null && databaseProvider == "MockDataStoreProvider")
            {
                builder.Services.AddSingleton(MockDatabase.Instance.DataStoreProvider);

                EnsureSystemConfigurationExists(MockDatabase.Instance.DataStoreProvider);
            }
            else
            {
                InitializeDatabase(builder, config);
            }

            builder.Services.AddValidatorsFromAssemblyContaining<ScheduledJobValidator>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }

        private static void InitializeDatabase(WebApplicationBuilder builder, IConfigurationRoot config)
        {
            var mockDataStoreProvider = new MockDataStoreProvider();
            var sqliteDataStoreProvider = new SQLiteDataStoreProvider();

            var workingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var databaseRelativePath = Path.Combine(config["KronoMata:SQLite:DatabaseRootPath"], config["KronoMata:SQLite:DatabaseFileName"]);
#pragma warning disable CS8604 // Possible null reference argument.
            var databasePath = Path.Combine(workingDirectory, databaseRelativePath);
#pragma warning restore CS8604 // Possible null reference argument.

            if (!File.Exists(databasePath))
            {
                throw new ArgumentException($"Unable to find database at {databasePath}");
            }

            var connectionString = $"Data Source={databasePath};{config["KronoMata:SQLite:DatabaseOptions"]}";
            SQLiteDataStoreBase.ConnectionString = connectionString;

            var inMemoryDataStoreProvider = new InMemoryDataStoreProvider(mockDataStoreProvider, sqliteDataStoreProvider);
            EnsureSystemConfigurationExists(inMemoryDataStoreProvider);
            builder.Services.AddSingleton<IDataStoreProvider>(inMemoryDataStoreProvider);
            
        }

        private static void EnsureSystemConfigurationExists(IDataStoreProvider dataStoreProvider)
        {
            var now = DateTime.Now;

            const string categoryName = "JobHistory";
            const string maxDaysName = "MaxDays";
            const string maxRecordsName = "MaxRecords";

            var historyExpirationDays = dataStoreProvider.GlobalConfigurationDataStore.GetByCategoryAndName(categoryName, maxDaysName);
            var historyExpirationMax = dataStoreProvider.GlobalConfigurationDataStore.GetByCategoryAndName(categoryName, maxRecordsName);

            if (historyExpirationDays == null)
            {
                // 14 day max default for JobHistory
                CreateConfigurationValue(dataStoreProvider, categoryName, maxDaysName, now, "14");
            }

            if (historyExpirationMax == null)
            {
                // 20,000 record max for JobHistory
                CreateConfigurationValue(dataStoreProvider, categoryName, maxRecordsName, now, "10000");
            }
        }

        private static void CreateConfigurationValue(IDataStoreProvider dataStoreProvider, string category, string name, DateTime now, string configValue)
        {
            var configuration = new GlobalConfiguration
            {
                Category = category,
                InsertDate = now,
                IsAccessibleToPlugins = true,
                IsMasked = false,
                IsSystemConfiguration = true,
                Name = name,
                UpdateDate = now,
                Value = configValue
            };

            dataStoreProvider.GlobalConfigurationDataStore.Create(configuration);
        }
    }
}