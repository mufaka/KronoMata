using KronoMata.Data;
using KronoMata.Model;
using KronoMata.Public;
using KronoMata.Web.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.InteropServices;

namespace KronoMata.Web.Controllers
{
    public class PackageController : BaseController
    {
        private readonly ILogger<JobHistoryController> _logger;

        public PackageController(ILogger<JobHistoryController> logger, IDataStoreProvider dataStoreProvider, 
            IConfiguration configuration)
        {
            _logger = logger;
            DataStoreProvider = dataStoreProvider;
            Configuration = configuration;
        }

        public IActionResult Index()
        {
            var model = new PackageViewModel();
            model.ViewName = "Packages";

            try
            {
                model.Packages = DataStoreProvider.PackageDataStore.GetAll()
                    .OrderBy(x => x.Name).ToList(); ;
            }
            catch (Exception ex)
            {
                LogException(model, ex);
                _logger.LogError(ex, ex.Message);
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Index(string packageName, IFormFile file)
        {
            var model = new PackageViewModel();
            model.ViewName = "Packages";

            try
            {
                var packageRoot = Configuration.GetValue<string>("PackageRoot");
                
                if (!Directory.Exists(packageRoot))
                {
                    Directory.CreateDirectory(packageRoot);
                }
                
                var uniqueName = $"{Guid.NewGuid()}.{Path.GetExtension(file.FileName)}";

                var package = new Package()
                {
                    Name = packageName,
                    FileName = uniqueName,
                    UploadDate = DateTime.Now
                };

                var packagePath = Path.Combine(packageRoot, uniqueName);

                using (var fs = System.IO.File.OpenWrite(packagePath))
                {
                    file.CopyTo(fs);
                }

                DataStoreProvider.PackageDataStore.Create(package);

                // need to extract the archive locally and find IPlugin implementations
                ExtractAndDiscoverPlugins(packagePath);

                // shouldn't need to do this here, should Redirect to Url.Xxx
                model.Packages = DataStoreProvider.PackageDataStore.GetAll()
                    .OrderBy(x => x.Name).ToList();
            }
            catch (Exception ex)
            {
                LogException(model, ex);
                _logger.LogError(ex, ex.Message);

                return View(model);
            }

            return View(model);
        }

        private void ExtractAndDiscoverPlugins(string packagePath)
        {
#pragma warning disable CS8604 // Possible null reference argument.
            var extractionFolder = Path.Combine(Path.GetDirectoryName(packagePath), Path.GetFileNameWithoutExtension(packagePath));
#pragma warning restore CS8604 // Possible null reference argument.

            ZipFile.ExtractToDirectory(packagePath, extractionFolder);

            var pluginFiles = Directory.GetFiles(extractionFolder);

            // The resolver needs runtime files included as well
            string[] runtimeAssemblies = Directory.GetFiles(RuntimeEnvironment.GetRuntimeDirectory(), "*.dll");
            var paths = new List<string>(runtimeAssemblies);

            // need to add path to KronoMata.Public dll
            var pluginType = typeof(IPlugin);
            paths.Add(pluginType.Assembly.Location);

            // need to add the packageFiles too
            paths.AddRange(pluginFiles);

            var resolver = new PathAssemblyResolver(paths);
            var ctx = new MetadataLoadContext(resolver);

            foreach (var pluginFile in pluginFiles)
            {
                var assembly = ctx.LoadFromAssemblyPath(pluginFile);

                var assemblyTypes = assembly.GetTypes();

                foreach (Type type in assemblyTypes)
                {
                    // type must be a class
                    if (type.IsClass)
                    {
                        // are any of the interfaces IPlugin?
                        var interfaces = type.GetInterfaces();

                        foreach (Type faces in interfaces)
                        {
                            if (faces.FullName == "KronoMata.Public.IPlugin")
                            {
                                Console.WriteLine("Found a KronoMata IPlugin implementation");
                                break;
                            }
                        }
                    }
                }
            } 
        }

        private void CreatePlugin()
        {

        }
    }
}
