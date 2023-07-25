using KronoMata.Model;

namespace KronoMata.Data.Mock
{
    public class MockHostDataStore : IHostDataStore
    {
        public Host Create(Host host)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<Host> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<Host> GetByMachineName(string machineName)
        {
            throw new NotImplementedException();
        }

        public void Update(Host host)
        {
            throw new NotImplementedException();
        }
    }
}
