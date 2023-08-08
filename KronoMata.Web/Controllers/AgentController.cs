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
        private IDataStoreProvider DataStoreProvider { get; set; }

        public AgentController(ILogger<AgentController> logger, IDataStoreProvider dataStoreProvider)
        {
            _logger = logger;
            DataStoreProvider = dataStoreProvider;
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
                    jobs = DataStoreProvider.ScheduledJobDataStore.GetByHost(host.Id).Where(h => h.IsEnabled).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
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
                _logger.LogError(ex, ex.Message);
            }

            return history;
        }
    }
}
