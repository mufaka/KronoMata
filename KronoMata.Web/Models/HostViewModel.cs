using KronoMata.Model;

namespace KronoMata.Web.Models
{
    public class HostViewModel : BaseViewModel
    {
        public List<Model.Host> Hosts { get; set; } = new List<Model.Host>();
    }
}
