using KronoMata.Data;
using KronoMata.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace KronoMata.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentController : ControllerBase
    {
        private readonly ILogger<AgentController> _logger;
#pragma warning disable IDE0052 // Remove unread private members
        private readonly IConfiguration _configuration;
#pragma warning restore IDE0052 // Remove unread private members
        private IDataStoreProvider DataStoreProvider { get; set; }

        public AgentController(ILogger<AgentController> logger, IDataStoreProvider dataStoreProvider,
            IConfiguration configuration)
        {
            _logger = logger;
            DataStoreProvider = dataStoreProvider;
            _configuration = configuration;
        }

        [HttpGet("jobs/{name}")]
        public IEnumerable<ScheduledJob> GetByHostName(string name)
        {
            var jobs = new List<ScheduledJob>();

            try
            {
                var host = DataStoreProvider.HostDataStore.GetByMachineName(name);

                if (host != null)
                {
                    if (host.IsEnabled)
                    {
                        jobs = DataStoreProvider.ScheduledJobDataStore.GetByHost(host.Id).Where(h => h.IsEnabled).ToList();
                    }
                    else
                    {
                        // host is not enabled, return empty job list.
                        return jobs;
                    }
                } 
                else
                {
                    var now = DateTime.Now;

                    var newHost = new Model.Host()
                    {
                        MachineName = name,
                        IsEnabled = false,
                        InsertDate = now,
                        UpdateDate = now
                    };

                    DataStoreProvider.HostDataStore.Create(newHost);

                    // new host is not enabled, return empty job list.
                    return jobs;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting host by name {name}", name);
            }

            return jobs;
        }

        [HttpPost("history")]
        public JobHistory Post([FromBody] JobHistory history)
        {
            try
            {
                DataStoreProvider.JobHistoryDataStore.Create(history);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating JobHistory");
            }

            return history;
        }
    }
}
