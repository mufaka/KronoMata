using FluentValidation;
using KronoMata.Data;
using KronoMata.Data.Mock;
using KronoMata.Data.SQLite;
using KronoMata.Model.Validation;

namespace KronoMata.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            //builder.Services.AddSingleton(MockDatabase.Instance.DataStoreProvider);
            builder.Services.AddSingleton<IDataStoreProvider>(new SQLiteDataStoreProvider());
            builder.Services.AddValidatorsFromAssemblyContaining<ScheduledJobValidator>();

            var databasePath = Path.Combine("Database", "KronoMata.db");
            SQLiteDataStoreBase.ConnectionString = $"Data Source={databasePath};Pooling=True;Cache Size=4000;Page Size=1024;FailIfMissing=True;Journal Mode=Off;";

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