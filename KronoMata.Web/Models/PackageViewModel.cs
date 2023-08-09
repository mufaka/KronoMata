using KronoMata.Model;

namespace KronoMata.Web.Models
{
    public class PackageViewModel : BaseViewModel
    {
        public List<Package> Packages { get; set; } = new List<Package>();
    }
}
