using KronoMata.Model;

namespace KronoMata.Data.Mock
{
    public class MockPackageDataStore : IPackageDataStore
    {
        public Package Create(Package package)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<Package> GetAll()
        {
            throw new NotImplementedException();
        }

        public Package GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Package package)
        {
            throw new NotImplementedException();
        }
    }
}
