using KronoMata.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;

namespace KronoMata.Agent
{
    internal class ApiClient
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public ApiClient(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public Model.Host? GetHost(string machineName)
        {
            var endPoint = $"Agent/host/{machineName}";
            var hosts = Get<Host>(endPoint);

            return hosts.Count == 0 ? null : hosts[0];
        }

        public PluginMetaData? GetPluginMetaData(int id)
        {
            var endPoint = $"Agent/plugin/{id}";
            var plugins = Get<PluginMetaData>(endPoint);

            return plugins.Count == 0 ? null : plugins[0];
        }

        public Package? GetPackage(int id)
        {
            var endPoint = $"Agent/package/{id}";
            var packages = Get<Package>(endPoint);

            return packages.Count == 0 ? null : packages[0];
        }

        public List<GlobalConfiguration> GetGlobalConfigurations()
        {
            var endPoint = "Agent/globalconfig";
            return Get<GlobalConfiguration>(endPoint);
        }

        public List<PluginConfiguration> GetPluginConfigurations(int pluginMetaDataId)
        {
            var endPoint = $"Agent/pluginconfig/{pluginMetaDataId}";
            return Get<PluginConfiguration>(endPoint);
        }

        public List<ConfigurationValue> GetConfigurationValues(int scheduledJobId)
        {
            var endPoint = $"Agent/configvalue/{scheduledJobId}";
            return Get<ConfigurationValue>(endPoint);
        }

        public List<ScheduledJob> GetScheduledJobs(string machineName)
        {
            var endpoint = $"Agent/jobs/{machineName}";
            return Get<ScheduledJob>(endpoint);
        }

        public void SaveJobHistory(JobHistory jobHistory)
        {
            var endpoint = $"Agent/history";
            Post<JobHistory>(endpoint, jobHistory);
        }

        public async Task FetchPackageFile(Package package, string packageRootPath)
        {
            var endpoint = $"Agent/package/file/{package.Id}";
            var httpClient = new HttpClient();

            var destinationPath = Path.Combine(packageRootPath, package.FileName);

            var stream = await httpClient.GetStreamAsync(BuildUrl(endpoint));

            var fileStream = new FileStream(destinationPath, FileMode.Create);
            await stream.CopyToAsync(fileStream);

            fileStream.Flush();
            fileStream.Close();
        }

        private string RootUrl
        {
            get
            {
                var rootUrl = _configuration["KronoMata:APIRoot"];

                if (String.IsNullOrEmpty(rootUrl))
                {
                    throw new ArgumentNullException("APIRoot is not defined in appsettings.json [KronoMata:APIRoot]");
                }

                return rootUrl;
            }
        }

        private string BuildUrl(string endPoint)
        {
            if (RootUrl.EndsWith("/") && !endPoint.StartsWith("/"))
            {
                return RootUrl + endPoint;
            }
            else if (!RootUrl.EndsWith("/") && endPoint.StartsWith("/"))
            {
                return RootUrl + endPoint;
            }
            else if (RootUrl.EndsWith("/") && endPoint.StartsWith("/"))
            {
                return string.Concat(RootUrl, endPoint.AsSpan(1));
            }
            else
            {
                return RootUrl + "/" + endPoint;
            }
        }

        public List<T> Get<T>(string endPoint)
        {
            var client = _httpClientFactory.CreateClient();

            // why is there no Get? have to use Async?
            var response = client.GetAsync(BuildUrl(endPoint));

            // synchronous hackery ... :( ... slow AF on windows but not MAC ...
            response.Wait();
            var content = response.Result.Content.ReadAsStringAsync();
            content.Wait();

#pragma warning disable CS8603 // Possible null reference return.
            return JsonConvert.DeserializeObject<List<T>>(content.Result);
#pragma warning restore CS8603 // Possible null reference return.
        }

        public T Post<T>(string endPoint, T t)
        {
            var client = _httpClientFactory.CreateClient();
            var content = new StringContent(JsonConvert.SerializeObject(t), Encoding.UTF8, "application/json");
            var response = client.PostAsync(BuildUrl(endPoint), content);
            response.Wait();
            var responseContent = response.Result.Content.ReadAsStringAsync();
            responseContent.Wait();

#pragma warning disable CS8603 // Possible null reference return.
            return JsonConvert.DeserializeObject<T>(responseContent.Result);
#pragma warning restore CS8603 // Possible null reference return.
        }
    }
}
