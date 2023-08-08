using KronoMata.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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


    }
}
