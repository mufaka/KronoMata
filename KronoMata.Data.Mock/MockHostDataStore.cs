using KronoMata.Model;

namespace KronoMata.Data.Mock
{
    public class MockHostDataStore : BaseMockDataStore, IHostDataStore
    {
        public MockHostDataStore(MockDataStoreProvider dataProvider) : base(dataProvider) { }

        private readonly List<Host> _hosts = new();

        public Host Create(Host host)
        {
            host.Id = _hosts.Count == 0
                ? 1
                : _hosts[^1].Id + 1;

            _hosts.Add(host);

            return host;
        }

        public void Delete(int id)
        {
            //_hosts.RemoveAll(h => h.Id == id);

            var existingIndex = _hosts.FindIndex(g => g.Id == id);

            if (existingIndex >= 0)
            {
                _hosts.RemoveAt(existingIndex);
            }
        }

        public Host GetById(int id)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return _hosts.Where(h => h.Id == id).FirstOrDefault();
#pragma warning restore CS8603 // Possible null reference return.
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
                var existingIndex = _hosts.FindIndex(g => g.Id == existing.Id);

                if (existingIndex >= 0)
                {
                    _hosts[existingIndex] = host;
                }
            }
        }
    }
}
