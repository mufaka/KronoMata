using KronoMata.Public;
using System.Net;

namespace KronoMata.Plugins.Admin
{
    public class JobHistoryExpiration : IPlugin
    {
        public string Name { get { return "Job History Expiration"; } }

        public string Description { get { return "Calls the Job History Expiration endpoint."; } }

        public string Version { get { return "1.0"; } }

        public List<PluginParameter> Parameters
        {
            get
            {
                var parameters = new List<PluginParameter>();

                parameters.Add(new PluginParameter()
                {
                    Name = "KronoMata.Web Url",
                    Description = "The root url of the KronoMata.Web instance, including port. Eg: http://10.10.10.80:5002/",
                    DataType = ConfigurationDataType.String,
                    IsRequired = true
                });

                return parameters;
            }
        }

        private PluginResult? ValidateRequiredParameters(Dictionary<string, string> pluginConfig)
        {
            PluginResult? missingRequiredParameterResult = null;

            foreach (PluginParameter parameter in Parameters)
            {
                if (parameter.IsRequired && !pluginConfig.ContainsKey(parameter.Name))
                {
                    missingRequiredParameterResult ??= new PluginResult()
                    {
                        IsError = true,
                        Message = "Missing required parameter(s).",
                        Detail = "The plugin configuration is missing the following parameters:"
                    };

                    missingRequiredParameterResult.Detail = missingRequiredParameterResult.Detail + Environment.NewLine + parameter.Name;
                }
            }

            return missingRequiredParameterResult;
        }

        public List<PluginResult> Execute(Dictionary<string, string> systemConfig, Dictionary<string, string> pluginConfig)
        {
            var log = new List<PluginResult>();

            try
            {
                var invalidConfigurationResult = ValidateRequiredParameters(pluginConfig);

                if (invalidConfigurationResult != null)
                {
                    log.Add(invalidConfigurationResult);
                }
                else
                {
                    var url = pluginConfig["KronoMata.Web Url"];

                    if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                    {
                        throw new ArgumentException("Invalid url provided. It must be well formed and absolute. Eg: http://10.10.10.80:5002/");
                    }

                    var endpoint = url + "JobHistory/Expire";

                    if (!url.EndsWith("/"))
                    {
                        endpoint = $"{url}/JobHistory/Expire";
                    }

                    var uri = new Uri(endpoint);

                    if (uri.Scheme.ToLower() != "http" && uri.Scheme.ToLower() != "https")
                    {
                        throw new ArgumentException("The Uri scheme must be either http or https.");
                    }
                    else if (uri.Scheme.ToLower() == "https")
                    {
                        ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    }

                    var client = new HttpClient();
                    var request = new HttpRequestMessage(HttpMethod.Get, endpoint);

                    var response = client.Send(request);

                    if (StatusCodeMeansSuccess(response.StatusCode))
                    {
                        using var reader = new StreamReader(response.Content.ReadAsStream());
                        var result = reader.ReadToEnd();

                        log.Add(new PluginResult()
                        {
                            IsError = false,
                            Message = $"Request Successful ({response.StatusCode})",
                            Detail = $"{result} JobHistory records were expired."
                        });
                    }
                    else
                    {
                        log.Add(new PluginResult()
                        {
                            IsError = true,
                            Message = $"Received HTTP Status Code {response.StatusCode}",
                            Detail = "For a complete list of status codes and their meaning, visit https://learn.microsoft.com/en-us/dotnet/api/system.net.httpstatuscode?view=net-6.0"
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                log.Add(new PluginResult()
                {
                    IsError = true,
                    Message = ex.Message,
                    Detail = ex.StackTrace ?? String.Empty
                });
            }

            return log;
        }

        private bool StatusCodeMeansSuccess(HttpStatusCode statusCode)
        {
            // https://www.rfc-editor.org/rfc/rfc9110.html#name-status-codes
            // https://datatracker.ietf.org/doc/html/rfc2616
            // https://learn.microsoft.com/en-us/dotnet/api/system.net.httpstatuscode?view=net-6.0
            switch (statusCode)
            {
                // Successful 2xx
                case HttpStatusCode.OK:                             // 200
                case HttpStatusCode.Created:                        // 201
                case HttpStatusCode.Accepted:                       // 202
                case HttpStatusCode.NonAuthoritativeInformation:    // 203
                case HttpStatusCode.NoContent:                      // 204
                case HttpStatusCode.ResetContent:                   // 205
                case HttpStatusCode.PartialContent:                 // 206
                case HttpStatusCode.MultipleChoices:                // 207
                case HttpStatusCode.AlreadyReported:                // 208
                case HttpStatusCode.IMUsed:                         // 226
                    return true;
                default:
                    return false;
            }
        }
    }
}
