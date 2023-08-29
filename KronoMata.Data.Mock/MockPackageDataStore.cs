using KronoMata.Model;

namespace KronoMata.Data.Mock
{
    public class MockPackageDataStore : BaseMockDataStore, IPackageDataStore
    {
        public MockPackageDataStore(MockDataStoreProvider dataProvider) : base(dataProvider) { }

        private List<Package> _packages = new();

        public void Initialize(List<Package> packages) { _packages = packages; }

        public Package Create(Package package)
        {
            if (package.Id <= 0)
            {
                package.Id = _packages.Count == 0
                    ? 1
                    : _packages[^1].Id + 1;
            }

            _packages.Add(package);

            return package;
        }

        public void Delete(int id)
        {
            //_packages.RemoveAll(p => p.Id == id);

            var existingIndex = _packages.FindIndex(g => g.Id == id);

            if (existingIndex >= 0)
            {
                _packages.RemoveAt(existingIndex);
            }
        }

        public List<Package> GetAll()
        {
            return _packages;
        }

        public Package GetById(int id)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return _packages.Where(p => p.Id == id).FirstOrDefault();
#pragma warning restore CS8603 // Possible null reference return.
        }

        public void Update(Package package)
        {
            var existing = _packages.Where(p => p.Id == package.Id).FirstOrDefault();

            if (existing != null)
            {
                var existingIndex = _packages.FindIndex(g => g.Id == existing.Id);

                if (existingIndex >= 0)
                {
                    _packages[existingIndex] = package;
                }
            }
        }
    }
}
