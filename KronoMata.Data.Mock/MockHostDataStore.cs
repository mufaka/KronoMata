using KronoMata.Model;

namespace KronoMata.Data.Mock
{
    public class MockHostDataStore : IHostDataStore
    {
        private List<Host> _hosts = new List<Host>();

        public Host Create(Host host)
        {
            host.Id = _hosts.Count == 0
                ? 1
                : _hosts[_hosts.Count - 1].Id + 1;

            _hosts.Add(host);

            return host;
        }

        public void Delete(int id)
        {
            _hosts.RemoveAll(h => h.Id == id);
        }

        public List<Host> GetAll()
        {
            return _hosts;
        }

        public Host GetByMachineName(string machineName)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return _hosts.Where(h => h.MachineName == machineName).FirstOrDefault();
#pragma warning restore CS8603 // Possible null reference return.
        }

        public void Update(Host host)
        {
            var existing = _hosts.Where(h => h.Id == host.Id).FirstOrDefault();

            if (existing != null)
            {
                existing = host;
            }
        }
    }
}
