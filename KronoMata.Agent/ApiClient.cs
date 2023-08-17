using KronoMata.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;

namespace KronoMata.Agent
{
    internal class ApiClient
    {
        private readonly IConfiguration _configuration;

        public ApiClient(IConfiguration configuration)
        {
            _configuration = configuration;
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

        private string RootUrl
        {
            get
            {
                var configurationRoot = _configuration.GetSection("KronoMata");

                if (configurationRoot != null)
                {
                    var apiRoot = configurationRoot.GetSection("APIRoot");

                    if (apiRoot != null)
                    {
                        if (!String.IsNullOrEmpty(apiRoot.Value))
                        {
                            return apiRoot.Value;
                        }
                    }
                }

                throw new ArgumentNullException("APIRoot is not defined in appsettings.json [KronoMata:APIRoot]");
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
            using (var client = new HttpClient())
            {
                // why is there no Get? have to use Async?
                using (var response = client.GetAsync(BuildUrl(endPoint)))
                {
                    // synchronous hackery ... :(
                    response.Wait();
                    var content = response.Result.Content.ReadAsStringAsync();
                    content.Wait();

#pragma warning disable CS8603 // Possible null reference return.
                    return JsonConvert.DeserializeObject<List<T>>(content.Result);
#pragma warning restore CS8603 // Possible null reference return.
                }
            }
        }

        public T Post<T>(string endPoint, T t)
        {
            using (var client = new HttpClient())
            {
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
}
