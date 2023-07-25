using KronoMata.Model;

namespace KronoMata.Data
{
    /// <summary>
    /// Defines a DataStore for persisting Package.
    /// </summary>
    public interface IPackageDataStore
    {
        Package Create(Package package);
        void Update(Package package);
        void Delete(int id);
        Package GetById(int id);
        List<Package> GetAll();
    }
}
