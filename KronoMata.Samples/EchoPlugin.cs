using KronoMata.Public;

namespace KronoMata.Samples
{
    public class EchoPlugin : IPlugin
    {
        private const string MESSAGE_PARAMETER_NAME = "EchoMessage";
        private const string DETAIL_PARAMETER_NAME = "EchoDetail";

        public string Name { get { return "Echo Plugin"; } }
        public string Description { get { return "1.0";  } }
        public string Version { get { return "A plugin that echos configured text."; } }

        public List<PluginParameter> Parameters
        {
            get
            {
                var parameterList = new List<PluginParameter>
                {
                    new PluginParameter()
                    {
                        Name = MESSAGE_PARAMETER_NAME,
                        Description = "The message to echo to the log.",
                        DataType = ConfigurationDataType.String,
                        IsRequired = true
                    },

                    new PluginParameter()
                    {
                        Name = DETAIL_PARAMETER_NAME,
                        Description = "The detail to echo to the log.",
                        DataType = ConfigurationDataType.String,
                        IsRequired = true
                    }
                };

                return parameterList;
            }
        }

        private PluginResult? ValidateParameters(Dictionary<string, string> pluginConfig)
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
                var invalidConfigurationResult = ValidateParameters(pluginConfig);

                if (invalidConfigurationResult != null)
                {
                    log.Add(invalidConfigurationResult);
                }
                else
                {
                    log.Add(new PluginResult() 
                    {
                        IsError = false,
                        Message = pluginConfig[MESSAGE_PARAMETER_NAME],
                        Detail = pluginConfig[DETAIL_PARAMETER_NAME]
                    });
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
    }
}
