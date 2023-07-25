using KronoMata.Model;

namespace KronoMata.Data
{
    /// <summary>
    /// Defines a DataStore for persisting Host.
    /// </summary>
    public interface IHostDataStore
    {
        Host Create(Host host);
        void Update(Host host);
        void Delete(int id);
        List<Host> GetAll();
        List<Host> GetByMachineName(string machineName);
    }
}
