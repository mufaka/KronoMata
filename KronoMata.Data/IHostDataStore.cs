using KronoMata.Model;

namespace KronoMata.Data
{
    public interface IHostDataStore
    {
        Host Create(Host host);
        void Update(Host host);
        void Delete(int id);
        List<Host> GetAll();
        List<Host> GetByMachineName(string machineName);
    }
}
