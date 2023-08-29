using FluentValidation;
using KronoMata.Data;
using KronoMata.Data.InMemory;
using KronoMata.Data.Mock;
using KronoMata.Data.SQLite;
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
            }
            else
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

                builder.Services.AddSingleton<IDataStoreProvider>(new InMemoryDataStoreProvider(mockDataStoreProvider, sqliteDataStoreProvider));
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
    }
}