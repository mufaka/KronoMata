using KronoMata.Data;
using KronoMata.Model;
using KronoMata.Public;
using KronoMata.Web.Models;
using McMaster.NETCore.Plugins;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;
using System.Linq.Expressions;
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

                // need to extract the archive locally and find IPlugin implementations
                ExtractAndDiscoverPlugins(package, packagePath);

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

            // TODO: This form submission is in the context of Dropzone so it doesn't really apply to the users experience :/ 
            // TODO: Look at wrapping the Dropzone processQueue with some type of event handlers for success and fail ...
            // TODO: Definitely need the fail parts visible to the end users ... right now fails go to a black hole.

            return RedirectToAction("Index", "Plugin");
        }

        private void ExtractAndDiscoverPlugins(Package package, string packagePath)
        {
#pragma warning disable CS8604 // Possible null reference argument.
            var extractionFolder = Path.Combine(Path.GetDirectoryName(packagePath), Path.GetFileNameWithoutExtension(packagePath));
#pragma warning restore CS8604 // Possible null reference argument.

            ZipFile.ExtractToDirectory(packagePath, extractionFolder);

            var pluginFiles = Directory.GetFiles(extractionFolder, "*.dll");

            // The resolver needs runtime files included as well
            string[] runtimeAssemblies = Directory.GetFiles(RuntimeEnvironment.GetRuntimeDirectory(), "*.dll");
            var paths = new List<string>(runtimeAssemblies);

            // need to ensure path to KronoMata.Public dll
            if (!pluginFiles.Contains("KronoMata.Public.dll"))
            {
                //paths.Add(typeof(IPlugin).Assembly.Location);
            }

            // need to add the packageFiles too
            paths.AddRange(pluginFiles);

            var resolver = new PathAssemblyResolver(paths);
            var ctx = new MetadataLoadContext(resolver);

            List<Type> foundPluginTypes = new List<Type>();

            foreach (var pluginFile in pluginFiles)
            {
                try
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
                                    foundPluginTypes.Add(type);
                                }
                            }
                        }
                    }
                } 
                catch (Exception ex)
                {
                    // probably not a .net assembly file
                    _logger.LogDebug($"Couldn't load {pluginFile}. {ex.Message}");
                }
            }

            if (foundPluginTypes.Count > 0)
            {
                // create the Package
                var createdPackage = DataStoreProvider.PackageDataStore.Create(package);

                // create the plugins
                foreach (Type pluginType in foundPluginTypes)
                {
                    CreatePlugin(createdPackage, pluginType);
                }
            }
        }

        private void CreatePlugin(Package package, Type pluginType)
        {
            // need to be able to load the IPlugin and get it's name and parameters
            var assemblyPath = pluginType.Assembly.Location;
            var pluginLoader = PluginLoader.CreateFromAssemblyFile(
                assemblyFile: assemblyPath,
                sharedTypes: new[] { typeof(IPlugin) },
                isUnloadable: true);
            var assembly = pluginLoader.LoadDefaultAssembly();

            if (assembly != null)
            {
#pragma warning disable CS8601 // Possible null reference argument.
#pragma warning disable CS8602 // Possible null reference argument.
#pragma warning disable CS8604 // Possible null reference argument.
                var plugin = assembly.CreateInstance(pluginType.FullName) as IPlugin;

                var now = DateTime.Now;

                var pluginMetaData = new PluginMetaData();

                pluginMetaData.Name = plugin.Name;
                pluginMetaData.Description = plugin.Description;
                pluginMetaData.Version = plugin.Version;
                pluginMetaData.AssemblyName = assembly.FullName;
                pluginMetaData.ClassName = pluginType.AssemblyQualifiedName;
                pluginMetaData.PackageId = package.Id;
                pluginMetaData.InsertDate = now;
                pluginMetaData.UpdateDate = now;

                var createdPluginMetaData = DataStoreProvider.PluginMetaDataDataStore.Create(pluginMetaData);

                foreach (PluginParameter parameter in plugin.Parameters)
                {
                    var pluginConfig = new PluginConfiguration();

                    pluginConfig.PluginMetaDataId = createdPluginMetaData.Id;
                    pluginConfig.Name = parameter.Name;
                    pluginConfig.Description = parameter.Description;
                    pluginConfig.DataType = parameter.DataType;
                    pluginConfig.IsRequired = parameter.IsRequired;
                    pluginConfig.InsertDate = now;
                    pluginConfig.UpdateDate = now;

                    DataStoreProvider.PluginConfigurationDataStore.Create(pluginConfig);
                }
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8602 // Possible null reference argument.
#pragma warning restore CS8601 // Possible null reference argument.

            }
        }
    }
}
